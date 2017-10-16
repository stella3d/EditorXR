#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.EditorVR.UI;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.InputNew;
using XRAuthoring;

namespace UnityEditor.Experimental.EditorVR.Proxies
{
    using ButtonDictionary = Dictionary<VRControl, List<ProxyHelper.ButtonObject>>;

    public class ProxyFeedbackRequest : FeedbackRequest
    {
        public int priority;
        public int controlIndex;
        public Node node;
        public string tooltipText;
    }

    abstract class TwoHandedProxyBase<T> : MonoBehaviour, IProxy, IFeedbackReceiver, ISetTooltipVisibility, ISetHighlight, IConnectInterfaces
        where T : TrackedController
    {
        const float k_FeedbackDuration = 5f;

        [SerializeField]
        protected GameObject m_LeftHandProxyPrefab;

        [SerializeField]
        protected GameObject m_RightHandProxyPrefab;

        [SerializeField]
        protected PlayerInput m_PlayerInput;

        protected Transform m_LeftHand;
        protected Transform m_RightHand;
        readonly List<ProxyFeedbackRequest> m_FeedbackRequests = new List<ProxyFeedbackRequest>();

        protected Dictionary<Node, Transform> m_RayOrigins;

        bool m_Hidden;

        readonly Dictionary<Node, ButtonDictionary> m_Buttons = new Dictionary<Node, ButtonDictionary>();

        bool m_Active;
        protected TrackedController m_LeftController;
        protected TrackedController m_RightController;

        public Transform leftHand
        {
            get { return m_LeftHand; }
        }

        public Transform rightHand
        {
            get { return m_RightHand; }
        }

        public virtual Dictionary<Node, Transform> rayOrigins
        {
            get { return m_RayOrigins; }
        }

        public virtual TrackedObject trackedObjectInput { protected get; set; }

        public bool active
        {
            get { return m_Active; }
            set
            {
                m_Active = value;
                if (activeChanged != null)
                    activeChanged();
            }
        }

        public event Action activeChanged;

        public virtual bool hidden
        {
            set
            {
                if (value != m_Hidden)
                {
                    m_Hidden = value;
                    m_LeftHand.gameObject.SetActive(!value);
                    m_RightHand.gameObject.SetActive(!value);
                }
            }
        }

        public Dictionary<Transform, Transform> menuOrigins { get; set; }
        public Dictionary<Transform, Transform> alternateMenuOrigins { get; set; }
        public Dictionary<Transform, Transform> previewOrigins { get; set; }
        public Dictionary<Transform, Transform> fieldGrabOrigins { get; set; }

        // Local method use only -- created here to reduce garbage collection
        static readonly List<Tooltip> k_TooltipList = new List<Tooltip>();

        public virtual void Awake()
        {
            m_LeftHand = ObjectUtils.Instantiate(m_LeftHandProxyPrefab, transform).transform;
            m_RightHand = ObjectUtils.Instantiate(m_RightHandProxyPrefab, transform).transform;
            var leftProxyHelper = m_LeftHand.GetComponent<ProxyHelper>();
            var rightProxyHelper = m_RightHand.GetComponent<ProxyHelper>();

            m_Buttons[Node.LeftHand] = GetButtonDictionary(leftProxyHelper);
            m_Buttons[Node.RightHand] = GetButtonDictionary(rightProxyHelper);

            m_RayOrigins = new Dictionary<Node, Transform>
            {
                { Node.LeftHand, leftProxyHelper.rayOrigin },
                { Node.RightHand, rightProxyHelper.rayOrigin }
            };

            menuOrigins = new Dictionary<Transform, Transform>()
            {
                { leftProxyHelper.rayOrigin, leftProxyHelper.menuOrigin },
                { rightProxyHelper.rayOrigin, rightProxyHelper.menuOrigin },
            };

            alternateMenuOrigins = new Dictionary<Transform, Transform>()
            {
                { leftProxyHelper.rayOrigin, leftProxyHelper.alternateMenuOrigin },
                { rightProxyHelper.rayOrigin, rightProxyHelper.alternateMenuOrigin },
            };

            previewOrigins = new Dictionary<Transform, Transform>
            {
                { leftProxyHelper.rayOrigin, leftProxyHelper.previewOrigin },
                { rightProxyHelper.rayOrigin, rightProxyHelper.previewOrigin }
            };

            fieldGrabOrigins = new Dictionary<Transform, Transform>
            {
                { leftProxyHelper.rayOrigin, leftProxyHelper.fieldGrabOrigin },
                { rightProxyHelper.rayOrigin, rightProxyHelper.fieldGrabOrigin }
            };

            InputSystem.onDeviceConnectedDisconnected += OnDeviceConnectedDisconnected;
            m_LeftController = (T)InputSystem.LookupDeviceWithTagIndex(typeof(T), (int)TrackedController.Handedness.Left, true);
            m_RightController = (T)InputSystem.LookupDeviceWithTagIndex(typeof(T), (int)TrackedController.Handedness.Right, true);
        }

        static ButtonDictionary GetButtonDictionary(ProxyHelper helper)
        {
            var buttonDictionary = new ButtonDictionary();
            foreach (var button in helper.buttons)
            {
                List<ProxyHelper.ButtonObject> buttons;
                if (!buttonDictionary.TryGetValue(button.control, out buttons))
                {
                    buttons = new List<ProxyHelper.ButtonObject>();
                    buttonDictionary[button.control] = buttons;
                }

                buttons.Add(button);
            }
            return buttonDictionary;
        }

        public virtual IEnumerator Start()
        {
            while (m_LeftController == null || m_RightController == null)
                yield return null;

            // In standalone play-mode usage, attempt to get the TrackedObjectInput
            if (trackedObjectInput == null && m_PlayerInput)
                trackedObjectInput = m_PlayerInput.GetActions<TrackedObject>();

            var leftProxyHelper = m_LeftHand.GetComponent<ProxyHelper>();
            var rightProxyHelper = m_RightHand.GetComponent<ProxyHelper>();
            this.ConnectInterfaces(ObjectUtils.AddComponent<ProxyAnimator>(leftProxyHelper.gameObject), leftProxyHelper.rayOrigin);
            this.ConnectInterfaces(ObjectUtils.AddComponent<ProxyAnimator>(rightProxyHelper.gameObject), rightProxyHelper.rayOrigin);
        }

        public virtual void OnDestroy()
        {
            InputSystem.onDeviceConnectedDisconnected -= OnDeviceConnectedDisconnected;
        }

        public virtual void Update()
        {
            if (!active && m_LeftController != null && m_RightController != null)
            {
                active = true;
            }
            else if (active && m_LeftController == null || m_RightController == null)
            {
                active = false;
            }
            if (active)
            {
                m_LeftHand.localPosition = trackedObjectInput.leftPosition.vector3;
                m_LeftHand.localRotation = trackedObjectInput.leftRotation.quaternion;

                m_RightHand.localPosition = trackedObjectInput.rightPosition.vector3;
                m_RightHand.localRotation = trackedObjectInput.rightRotation.quaternion;
            }
        }

        public void AddFeedbackRequest(FeedbackRequest request)
        {
            var proxyRequest = request as ProxyFeedbackRequest;
            if (proxyRequest != null)
            {
                m_FeedbackRequests.Add(proxyRequest);
                ExecuteFeedback(proxyRequest);
            }
        }

        void ExecuteFeedback(ProxyFeedbackRequest changedRequest)
        {
            if (!active)
                return;

            foreach (var proxyNode in m_Buttons)
            {
                foreach (var kvp in proxyNode.Value)
                {
                    ProxyFeedbackRequest request = null;
                    foreach (var req in m_FeedbackRequests)
                    {
                        var matchChanged = req.node == changedRequest.node && req.controlIndex == changedRequest.controlIndex;
                        var reqVRControl = VRControlFromControlIndex(req.controlIndex);
                        var matchButton = req.node == proxyNode.Key && reqVRControl.HasValue && reqVRControl.Value == kvp.Key;
                        var sameCaller = req.caller == changedRequest.caller;
                        var priority = request == null || req.priority >= request.priority;
                        if (matchButton && priority && (matchChanged || sameCaller))
                            request = req;
                    }

                    if (request == null)
                        continue;

                    foreach (var button in kvp.Value)
                    {
                        if (button.renderer)
                            this.SetHighlight(button.renderer.gameObject, true, duration: k_FeedbackDuration);

                        if (button.transform)
                        {
                            var tooltipText = request.tooltipText;
                            if (!string.IsNullOrEmpty(tooltipText))
                            {
                                k_TooltipList.Clear();
                                button.transform.GetComponents(k_TooltipList);
                                foreach (var tooltip in k_TooltipList)
                                {
                                    tooltip.tooltipText = tooltipText;
                                    this.ShowTooltip(tooltip, true, k_FeedbackDuration);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RemoveFeedbackRequest(FeedbackRequest request)
        {
            var proxyRequest = request as ProxyFeedbackRequest;
            if (proxyRequest != null)
                RemoveFeedbackRequest(proxyRequest);
        }

        void RemoveFeedbackRequest(ProxyFeedbackRequest request)
        {
            Dictionary<VRControl, List<ProxyHelper.ButtonObject>> group;
            if (m_Buttons.TryGetValue(request.node, out group))
            {
                List<ProxyHelper.ButtonObject> buttons;
                var reqVRControl = VRControlFromControlIndex(request.controlIndex);
                if (reqVRControl.HasValue && group.TryGetValue(reqVRControl.Value, out buttons))
                {
                    foreach (var button in buttons)
                    {
                        if (button.renderer)
                            this.SetHighlight(button.renderer.gameObject, false);

                        if (button.transform)
                        {
                            k_TooltipList.Clear();
                            button.transform.GetComponents(k_TooltipList);
                            foreach (var tooltip in k_TooltipList)
                            {
                                tooltip.tooltipText = string.Empty;
                                this.HideTooltip(tooltip, true);
                            }
                        }
                    }
                }
            }
            m_FeedbackRequests.Remove(request);

            ExecuteFeedback(request);
        }

        public void ClearFeedbackRequests(IRequestFeedback caller)
        {
            var requests = caller == null
                ? new List<ProxyFeedbackRequest>(m_FeedbackRequests)
                : m_FeedbackRequests.Where(feedbackRequest => feedbackRequest.caller == caller).ToList();

            foreach (var feedbackRequest in requests)
            {
                RemoveFeedbackRequest(feedbackRequest);
            }
        }

        protected abstract VRControl? VRControlFromControlIndex(int controlIndex);

        void OnDeviceConnectedDisconnected(InputDevice device, bool connected)
        {
            if (connected)
            {
                var trackedController = device as T;
                if (trackedController == null)
                    return;

                switch (trackedController.hand)
                {
                    case TrackedController.Handedness.Left:
                        m_LeftController = trackedController;
                        break;
                    case TrackedController.Handedness.Right:
                        m_RightController = trackedController;
                        break;
                }
            }
            else
            {
                if (device == m_LeftController)
                {
                    m_LeftController = null;
                }
                else if (device == m_RightController)
                {
                    m_RightController = null;
                }
            }
        }
    }
}
#endif
