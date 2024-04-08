using System;
using System.Collections;
using Units;
using Units.Hitting;
using UnityEngine;

namespace Animations
{
    [RequireComponent(typeof(Animator))]

    public class CatAnimator : MonoBehaviour
    {
        [SerializeField] private Cat _cat;

        [Range(0.01f, 0.3f)] [SerializeField] private float _enterStunDurationFraction;

        [Range(0.01f, 0.3f)] [SerializeField] private float _exitStunDurationFraction;

        [SerializeField] private SpriteRenderer _sprite;
        
        [Header("Hat")]
        [SerializeField] private SpriteRenderer _hatSprite;
        
        [SerializeField] private Sprite _rightHatSprite;
        [SerializeField] private Sprite _leftHatSprite;
        [SerializeField] private Sprite _upHatSprite;
        [SerializeField] private Sprite _downHatSprite;
        
        private Vector2 _previousPosition;

        private Animator _animator;

        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int SqrMagnitude = Animator.StringToHash("SqrMagnitude");

        private static readonly int Stagger = Animator.StringToHash("IsStaggered");
        private static readonly int StunEnter = Animator.StringToHash("StunEnter");
        private static readonly int Stun = Animator.StringToHash("IsStunned");
        private static readonly int StunExit = Animator.StringToHash("StunExit");
        private static readonly int Kickoff = Animator.StringToHash("Kickoff");
        private static readonly int AnimatorStunSpeed = Animator.StringToHash("AnimatorStunSpeed");
        
        private static readonly int IsCarryingFish = Animator.StringToHash("IsCarryingFish");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _cat.HitEvent += OnHit;
            
            _cat.FishStoleEvent += OnFishStolen;
            _cat.FishDroppedEvent += OnFishDropped;
        }

        private void OnDisable()
        {
            _cat.HitEvent -= OnHit;
            
            _cat.FishStoleEvent -= OnFishStolen;
            _cat.FishDroppedEvent -= OnFishDropped;
        }
        
        private void FixedUpdate()
        {
            Vector2 move = (Vector2)_cat.transform.position - _previousPosition;
            move = move.normalized;

            _animator.SetFloat(SqrMagnitude, move.sqrMagnitude);
            _animator.SetFloat(MoveX, move.x);
            _animator.SetFloat(MoveY, move.y);

            _previousPosition = _cat.transform.position;
        }

        #region AnimatorEvents
        
        private enum HatDirection : byte
        {
            Right,
            Left,
            Up,
            Down
        }

        private void OnWalkRight(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f && animationEvent.animatorStateInfo.normalizedTime > 0.5f)
            {
                return;
            }
            
            SetHatDirection(HatDirection.Right);
            
            if (_cat.CarriedFish == null)
            {
                return;
            }

            _cat.CarriedFish.Sprite.flipX = true;
            SetFishBehindCat();
        }
        
        private void OnWalkLeft(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f && animationEvent.animatorStateInfo.normalizedTime > 0.5f)
            {
                return;
            }
            
            SetHatDirection(HatDirection.Left);
            
            if (_cat.CarriedFish == null)
            {
                return;
            }
            
            _cat.CarriedFish.Sprite.flipX = false;
            SetFishBehindCat();
        }
        
        private void OnWalkUp(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f && animationEvent.animatorStateInfo.normalizedTime > 0.5f)
            {
                return;
            }
            
            SetHatDirection(HatDirection.Up);
            
            if (_cat.CarriedFish == null)
            {
                return;
            }
            
            SetFishBehindCat();
        }
        
        private void OnWalkDown(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f && animationEvent.animatorStateInfo.normalizedTime > 0.5f)
            {
                return;
            }
            
            SetHatDirection(HatDirection.Down);
            
            if (_cat.CarriedFish == null)
            {
                return;
            }
            
            SetFishInFrontOfCat();
        }

        private void SetHatDirection(HatDirection hatDirection)
        {
            if (_hatSprite == null)
            {
                return;
            }
            
            switch (hatDirection)
            {
                case HatDirection.Right:
                    _hatSprite.sprite = _rightHatSprite;
                    break;
                case HatDirection.Left:
                    _hatSprite.sprite = _leftHatSprite;
                    break;
                case HatDirection.Up:
                    _hatSprite.sprite = _upHatSprite;
                    break;
                case HatDirection.Down:
                    _hatSprite.sprite = _downHatSprite;
                    break;
            }
        }
        
        private void SetFishInFrontOfCat()
        {
            _cat.CarriedFish.Sprite.sortingOrder = _sprite.sortingOrder + 1;
        }
        
        private void SetFishBehindCat()
        {
            _cat.CarriedFish.Sprite.sortingOrder = _sprite.sortingOrder - 1;
        }

        #endregion
        
        private void OnFishStolen()
        {
            _animator.SetBool(IsCarryingFish, true);
        }
        
        private void OnFishDropped()
        {
            _animator.SetBool(IsCarryingFish, false);
        }
        
        private void OnHit(HitType hitType)
        {
            if (hitType == HitType.None)
            {
                return;
            }

            StopAllCoroutines();

            switch (hitType)
            {
                case HitType.Slap:
                    StartCoroutine(HandleStagger());
                    break;
                case HitType.Snap:
                    StartCoroutine(HandleStun());
                    break;
                case HitType.GigaSnap:
                    _animator.SetTrigger(Kickoff);
                    break;
            }
        }

        private IEnumerator HandleStagger()
        {
            _animator.SetBool(Stagger, true);

            yield return new WaitForSeconds((float)_cat.StaggerDuration.TotalSeconds);

            _animator.SetBool(Stagger, false);
        }

        private IEnumerator HandleStun()
        {
            _animator.SetTrigger(StunEnter);

            var duration = (float)_cat.StunDuration.TotalSeconds;
            
            float enterStunDuration = _enterStunDurationFraction * duration;
            _animator.SetFloat(AnimatorStunSpeed, 1f / enterStunDuration);

            yield return new WaitForSeconds(enterStunDuration);
            
            _animator.SetBool(Stun, true);
            
            _animator.SetFloat(AnimatorStunSpeed, 1f);
            float stunDuration = (1f - (_exitStunDurationFraction + _enterStunDurationFraction)) * duration;
            yield return new WaitForSeconds(stunDuration);

            _animator.SetBool(Stun, false);
            
            float exitStunDuration = _animator.GetCurrentAnimatorStateInfo(0).length * _exitStunDurationFraction * duration;
            _animator.SetFloat(AnimatorStunSpeed, 1f / exitStunDuration);

            yield return new WaitForSeconds(exitStunDuration);

            _animator.SetTrigger(StunExit);
            _animator.SetFloat(AnimatorStunSpeed, 1f);
        }
    }
}
