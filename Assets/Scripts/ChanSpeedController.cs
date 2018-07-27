using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 控制速度值的起步，加速，减速
	/// </summary>
	public class ChanSpeedController : MonoBehaviour {

		/// <summary>
		/// 速度改变的委托
		/// </summary>
		/// <param name="spd">速度值</param>
		private delegate void VelocityChangeHandler (float spd);

		private VelocityChangeHandler VelocityChange;

		/// <summary>
		/// 速度的精度误差，如果两个速度之间的差值小于这个值，就判断为速度相等
		/// </summary>
		public static float velocityError = 0.05f;

		/// <summary>
		/// 目标速度	
		/// </summary>
		[SerializeField]
		private float targetVelocity;

		/// <summary>
		/// 当前速度
		/// </summary>
		[SerializeField]
		private float _currentVelocity;

		/// <summary>
		/// 当前加速度
		/// </summary>
		[SerializeField]
		private float currentAcceleration;

		/// <summary>
		/// 最小加速度
		/// </summary>
		[SerializeField]
		private float minAcceleration = 1f;

		/// <summary>
		/// 最大加速度
		/// </summary>
		[SerializeField]
		private float maxAcceleration = 100f;

		/// <summary>
		/// 标准加速时间
		/// </summary>
		[SerializeField]
		private float standardAccelerateTime = 0.5f;

		/// <summary>
		/// 是否锁住速度改变
		/// </summary>
		[SerializeField]
		private bool lockVelocity = false;

		/// <summary>
		/// 当前速度值 （属性获取器）
		/// </summary>
		/// <value></value>
		public float currentVelocity {
			get { return _currentVelocity; }
		}

		private void FixedUpdate () {

			if (!lockVelocity) {
				float deltaSpeed = targetVelocity - _currentVelocity;

				if (Mathf.Abs (deltaSpeed) <= velocityError) {
					_currentVelocity = targetVelocity;
					currentAcceleration = 0;
					lockVelocity = true;
				} else {
					float acc = deltaSpeed / standardAccelerateTime;

					if (acc == 0) return;
					if (acc > 0) {
						currentAcceleration = Mathf.Clamp (acc, minAcceleration, maxAcceleration);
					} else {
						currentAcceleration = Mathf.Clamp (acc, -maxAcceleration, -minAcceleration);
					}
					_currentVelocity += currentAcceleration * Time.fixedDeltaTime;
				}
			}
		}

		/// <summary>
		/// 设置当前速度
		/// </summary>
		/// <param name="spd">当前速度值</param>
		/// <param name="isLock">是否锁定,默认False代表不锁定，设置完后由引擎自己管理速度值</param>
		public void SetCurrentVelocity (float spd, bool isLock = false) {
			_currentVelocity = spd;
			lockVelocity = isLock;
			if (VelocityChange != null)
				VelocityChange.Invoke (spd);
		}

		/// <summary>
		/// 使目标速度提高delta
		/// </summary>
		/// <param name="delta">提高值</param>
		public void SpeedUp (float delta) {
			SetCurrentVelocity (targetVelocity + delta, false);
		}

		/// <summary>
		/// 把当前速度设置为0，并锁定。
		/// </summary>
		public void Stop () {
			SetCurrentVelocity (0, true);
		}

		/// <summary>
		/// 使当前速度值可以被改变
		/// </summary>
		public void UnLock () {
			lockVelocity = false;
		}

		/// <summary>
		/// 使当前速度值不可被改变
		/// </summary>
		public void Lock () {
			lockVelocity = false;
		}
	}

}