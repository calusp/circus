using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Utils {
	public static class AnimatorExtensions 
	{
		public static IObservable<Unit> WaitUntilAnimationIsPlayedObservable(this Animator animator, int state, int layerIndex = 0)
		{
			return animator.WaitUntilAnimationIsPlayed(state, layerIndex).ToObservable();
		}

		private static IEnumerator WaitUntilAnimationIsPlayed(this Animator animator, int state, int layerIndex)
		{
			return animator.WaitUntilAnimationIsPlayed(new[] { state }, layerIndex);
		}

		private static IEnumerator WaitUntilAnimationIsPlayed(this Animator animator, int[] states, int layerIndex)
		{
			//Wait at least for one animator update so it triggers any pending transition;
			yield return null;

			while(
				animator != null 
				&& (!states.Contains(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash) || animator.IsInTransition(layerIndex)))
			{
				yield return null;
			}
		}

		public static IObservable<Unit> WaitUntilCurrentAnimationEndsAsObservable(this Animator animator, int layerIndex = 0, float normalizedTimeOffset = 0f)
		{
			return animator.WaitUntilCurrentAnimationEnds(layerIndex, normalizedTimeOffset).ToObservable();
		}

		public static IObservable<Unit> WaitUntilAnimationEndsAsObservable(this Animator animator, int[] states, int layerIndex = 0, float normalizedTimeOffset = 0f)
		{
			return animator.WaitUntilAnimationEnds(states, layerIndex, normalizedTimeOffset).ToObservable();
		}

		public static IObservable<Unit> WaitUntilAnimationEndsAsObservable(this Animator animator, int state, int layerIndex = 0, float normalizedTimeOffset = 0f)
		{
			return animator.WaitUntilAnimationEnds(state, layerIndex, normalizedTimeOffset).ToObservable();
		}

		private static IEnumerator WaitUntilAnimationEnds(this Animator animator, int state, int layerIndex, float normalizedTimeOffset)
		{
			yield return animator.WaitUntilAnimationEnds(new[] { state }, layerIndex, normalizedTimeOffset);
		}

		private static IEnumerator WaitUntilAnimationEnds(this Animator animator, int[] states, int layerIndex, float normalizedTimeOffset)
		{
			yield return animator.WaitUntilAnimationIsPlayed(states, layerIndex);

			while(
				animator != null 
				&& states.Contains(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash) 
				&& !animator.IsInTransition(layerIndex)
				&& animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime <= (0.95f - normalizedTimeOffset))
			{
				yield return null;
			}
		}

		private static IEnumerator WaitUntilCurrentAnimationEnds(this Animator animator, int layerIndex = 0, float normalizedTimeOffset = 0f)
		{
			yield return null;
			yield return animator.WaitUntilAnimationEnds(animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash, layerIndex, normalizedTimeOffset);
		}


		public static void SetTimeForAnimations(this Animation animation, float percent)
		{
			foreach (AnimationState animState in animation)
			{
				animState.normalizedTime = animState.length * percent;
			}
		}
	}
}