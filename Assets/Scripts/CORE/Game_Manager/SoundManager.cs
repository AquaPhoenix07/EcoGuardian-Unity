using System.Collections;
using UnityEngine;

namespace CORE.Game_Manager
{
    
    public class SoundManager : MonoBehaviour
    {
        //SingleTon để tạo ra 1 bản thể duy nhất trong xuyên suốt cả game
        public static SoundManager Instance { get; private set; }
        
        [Header("Sound Manager")]
        public AudioSource bmgSource;
        public AudioSource sfxSource;
        
        [Header("Sound Clips")]
        public AudioClip[] welcomeSound;
        public AudioClip restartSound;
        public AudioClip settingSound;
        public AudioClip missionSound;
        public AudioClip targetSound;
        
        public AudioClip backgroundMusic;
        
        [Header("Sound Settings")] 
        public bool isBackgroundMusicOn = true;
        
        private void Start()
        {
            if (backgroundMusic != null && bmgSource != null)
            {
                bmgSource.clip = backgroundMusic; 
                bmgSource.loop = true;            
                bmgSource.Play();                 
            }
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // Nếu đã có một Instance khác tồn tại (do bạn load lại màn cũ sinh ra thêm 1 bản sao), 
                // tiêu diệt bản sao dư thừa này ngay lập tức để giữ nguyên tắc "Duy Nhất".
                Destroy(gameObject);
            }
        }
        
        //RIGHT TOOL
        public IEnumerator PlayButtonClicked()
        {
            foreach (var clip in welcomeSound)
            {
               sfxSource.PlayOneShot(clip);
               yield return new WaitForSeconds(clip.length);
            }
        }
        public void SettingButtonClicked()
        {
            sfxSource.PlayOneShot(settingSound);
        }

        public void RestartButtonClicked()
        {
            sfxSource.PlayOneShot(restartSound);
        }

        public void MissionButtonClicked()
        {
            sfxSource.PlayOneShot(missionSound);
        }
        //BACKGROUND MUSIC
        
        public void BackgroundMusicToggle(bool isOn)
        {
            // isOn = true nghĩa là người chơi ĐANG MUỐN TẮT TIẾNG (Màu đỏ)
            if (isOn)
            {
                bmgSource.mute = true;
                isBackgroundMusicOn = false;
            }
            else if (!isOn)
            {
                bmgSource.mute = false;
                isBackgroundMusicOn = true;
            }
        }
        public void ChangeBackgroundMusic(AudioClip newClip)
        {
            if (bmgSource.clip == newClip) return;
            bmgSource.clip = newClip;
            bmgSource.loop = true;
            bmgSource.Play();
        }
        //ANOTHER SFX
        public void TargetSound()
        {
            sfxSource.PlayOneShot(targetSound);
        }
    }
}