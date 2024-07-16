using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace __Scripts
{
    public class VolumeSettings : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Slider general;
        public Slider musicSlider;
        public Slider effectsSlider;
        public GameObject mainMenu;
        public GameObject volumeMenu;

        public void Start()
        {
            general.value = SaveManager.Instance.generalVolume;
            musicSlider.value = SaveManager.Instance.musicVolume;
            effectsSlider.value = SaveManager.Instance.effectsVolume;

            SetGeneralVolume();
            SetMusicVolume();
            SetEffectsVolume();
        }
        public void SetGeneralVolume()
        {
            float volume = general.value;
            audioMixer.SetFloat("general", Mathf.Log10(volume) * 20);
            SaveManager.Instance.generalVolume = volume;
        }

        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            SaveManager.Instance.musicVolume = volume;
        }

        public void SetEffectsVolume()
        {
            float volume = effectsSlider.value;
            audioMixer.SetFloat("effects", Mathf.Log10(volume) * 20);
            SaveManager.Instance.effectsVolume = volume;
        }

        public void BackBtn()
        {
            mainMenu.SetActive(true);
            volumeMenu.SetActive(false);
        }
    }
}
