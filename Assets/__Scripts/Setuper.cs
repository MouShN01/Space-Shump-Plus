using System;

namespace __Scripts
{
    public class Setuper : Singleton<Setuper>
    {
        private void Start()
        {
            SaveManager.Instance.LoadData();
        }
    }
}