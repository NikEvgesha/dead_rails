using UnityEngine;

public class ControlUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _mobileUI;
    [SerializeField]
    private GameObject _desktopUI;

    [SerializeField] private OnScreenButton _upButton;
    [SerializeField] private OnScreenButton _downButton;
    [SerializeField] private OnScreenButton _jumpButton;

    private bool _isMobile;

    public void UseMobileSetup(bool isMobile)
    {
        _isMobile = isMobile;
        _mobileUI.SetActive(isMobile);
        _desktopUI.SetActive(!isMobile);
    }
    

    public void SwitchPlatformControls(bool onPlatform)
    {
        _upButton.gameObject.SetActive(!onPlatform);
        _downButton.gameObject.SetActive(!onPlatform);
        _jumpButton.gameObject.SetActive(onPlatform);
    }

}
