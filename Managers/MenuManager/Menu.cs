using UnityEngine;

namespace StrikeOnline.Managers.MenuManager
{
    public enum MenuEnum
    {
        MainMenu,
        CreateRoomMenu,
        RoomMenu,
        LoadingMenu,
        FindRoomMenu,
        ErrorMenu
    }

    public class Menu : MonoBehaviour
    {
        #region Public Fields

        public MenuEnum menuEnum;
        public bool open;

        #endregion

        #region Public Methods

        public void Open()
        {
            open = true;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            open = false;
            gameObject.SetActive(false);
        }

        #endregion
    }
}