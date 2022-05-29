using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
//using ItemCollection.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ItemCollection.GameEndUnlockItem
{
    [Serializable]
    public class ItemPack
    {
        public Sprite lockSprite;
        public Sprite unLockSprite;
    }

    public class UnlockItemWithPercentage : MonoBehaviour
    {
        private const string UnlockPercentageKey = "unlockPercentage";
        public const string NextPackIndexKey = "nextPackIndex";

        [SerializeField] private TextMeshProUGUI txtUnlockPercentage;

        [Space(20)] [SerializeField] private Image imgLocked;
        [SerializeField] private Image imgUnLocked;

        [Space(20)] [SerializeField] private Button btnReveal;
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnCollect;

        [Space(20)] [SerializeField] private List<ItemPack> allItemPack = new List<ItemPack>();

        private int _unlockPercentage = 0;
        private int _nextPackIndex = 0;
        public  int _increaseAmount = 20;
        private const float _duration = 2.0f;

        private void Awake()
        {
            _nextPackIndex = PlayerPrefs.GetInt(NextPackIndexKey, 0);

            if (_nextPackIndex > allItemPack.Count - 1)
            {
                Destroy(this.gameObject);
                NextCallBack();
                return;
            }

            AssignNextItem();

            _unlockPercentage = GetUnlockPercentage();
            imgUnLocked.fillAmount = _unlockPercentage / 100f;

            if (_unlockPercentage < 100)
                _unlockPercentage += _increaseAmount;

            StartCoroutine(DisplayItemPack());

            btnReveal.onClick.AddListener(RevealCallBack);
            btnNext.onClick.AddListener(NextCallBack);
            btnCollect.onClick.AddListener(CollectCallBack);

            btnReveal.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(false);
            btnCollect.gameObject.SetActive(false);
        }

        private void CollectCallBack()
        {
            PlayerPrefs.SetInt(NextPackIndexKey, PlayerPrefs.GetInt(NextPackIndexKey, 0) + 1);
            SaveUnlockPercentage(0);
            NextCallBack();
    
        }

        private void RevealCallBack()
        {
            // if (Application.isEditor)
            //     OnAdViewCompleteCallBack(true);
            // else
            //FPG.AdViewer.Instance.ShowVideoAdOrCrossPromo(OnAdViewCompleteCallBack, "RevealRewAd");
        }

        private void OnAdViewCompleteCallBack(bool success)
        {
            btnNext.gameObject.SetActive(false);
            btnReveal.gameObject.SetActive(false);

            if (success)
            {
                _unlockPercentage += _increaseAmount;
                StartCoroutine(DisplayItemPack(true));
            }
            else
                NextCallBack();
        }

        private void NextCallBack()
        {
            UiManager.Instance.Next();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void AssignNextItem()
        {
            imgLocked.sprite = allItemPack[_nextPackIndex].lockSprite;
            imgUnLocked.sprite = allItemPack[_nextPackIndex].unLockSprite;
        }

        private IEnumerator DisplayItemPack(bool shouldCallNext = false)
        {
            SaveUnlockPercentage(_unlockPercentage);
            float targetFillAmount = _unlockPercentage / 100f;

            //Debug.Log("FillAmount " + imgUnLocked.fillAmount + "  targetFillAmount " + targetFillAmount);
            DOTween.To(() => imgUnLocked.fillAmount, x => imgUnLocked.fillAmount = x, targetFillAmount, _duration);
            int previousPercentage = _unlockPercentage - _increaseAmount;
            DOTween.To(() => previousPercentage, x => previousPercentage = x, _unlockPercentage, _duration).OnUpdate(
                delegate { txtUnlockPercentage.text = previousPercentage + "%"; });

            if (_unlockPercentage < 100)
            {
                yield return new WaitForSeconds(_duration);
                if (shouldCallNext)
                {
                    //btnReveal.gameObject.SetActive(false);
                    //btnNext.gameObject.SetActive(false);
                    NextCallBack();
                    yield break;
                }

                btnReveal.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                btnNext.gameObject.SetActive(true);
                yield break;
            }

            //Claim Button SetActive True
            btnReveal.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(false);
            yield return new WaitForSeconds(_duration);
            btnCollect.gameObject.SetActive(true);
        }

        private void SaveUnlockPercentage(int percentage) => PlayerPrefs.SetInt(UnlockPercentageKey, percentage);
        private int GetUnlockPercentage() => PlayerPrefs.GetInt(UnlockPercentageKey, 0);
    }
}