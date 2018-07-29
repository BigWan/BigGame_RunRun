using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 控制速度值的起步，加速，减速
	/// </summary>
	public class ChanSpeedController : MonoBehaviour {


		public delegate void VelocityChangeHandler (float spd);

		/// <summary>
		/// 速度改变的委托
		/// </summary>
		/// <param name="spd">速度值</param>
		public VelocityChangeHandler VelocityChange;

		private enum SpeedStat{
			stop = 0,
			accelate = 1,
			moveing = 2,
		}


		/// <summary>
		/// 速度的精度误差，如果两个速度之间的差值小于这个值，就判断为速度相等
		/// </summary>
		public static float velocityError = 0.05f;

		/// <summary>
		/// 目标速度
		/// </summary>
		[SerializeField]
		private float _targetVelocity;


		[SerializeField]
		private float _currentVelocity;
		/// <summary>
		/// 当前速度值
		/// </summary>
		/// <value></value>
		public float currentVelocity {
			get { return _currentVelocity; }
			set {
				_currentVelocity = value;
				if (VelocityChange != null)
					VelocityChange.Invoke (value);
			}
		}

		/// <summary>
		/// 当前加速度
		/// </summary>
		[SerializeField]
		private float _currentAcceleration;

		/// <summary>
		/// 最小加速度
		/// </summary>
		[SerializeField]
		private float _minAcceleration = 1f;

		/// <summary>
		/// 最大加速度
		/// </summary>
		[SerializeField]
		private float _maxAcceleration = 100f;

		/// <summary>
		/// 标准加速时间
		/// </summary>
		[SerializeField]
		private float _standardAccelerateTime = 0.5f;

		/// <summary>
		/// 是否锁住速度改变
		/// </summary>
		[SerializeField]
		private bool _lockVelocity = false;

		private void FixedUpdate () {

			if (!_lockVelocity) {
				float deltaSpeed = _targetVelocity - currentVelocity;

				if (Mathf.Abs (deltaSpeed) <= velocityError) {
					SetCurrentVelocity(_targetVelocity,true);
					_currentAcceleration = 0;
				} else {
					float acc = deltaSpeed / _standardAccelerateTime;

					if (acc == 0) return;
					if (acc > 0) {
						_currentAcceleration = Mathf.Clamp (acc, _minAcceleration, _maxAcceleration);
					} else {
						_currentAcceleration = Mathf.Clamp (acc, -_maxAcceleration, -_minAcceleration);
					}
					currentVelocity += _currentAcceleration * Time.fixedDeltaTime;
				}
			}
		}



		/// <summary>
		/// 设置当前速度
		/// </summary>
		/// <param name="spd">当前速度值</param>
		/// <param name="isLock">是否锁定,默认False代表不锁定，设置完后由引擎自己管理速度值</param>
		public void SetCurrentVelocity (float spd, bool isLock = false) {
			currentVelocity = spd;
			_lockVelocity = isLock;

		}

		/// <summary>
		/// 使目标速度提高 delta
		/// </summary>
		/// <param name="delta">提高值</param>
		public void SpeedUp (float delta) {
			// SetCurrentVelocity (_targetVelocity + delta, false);
			_targetVelocity +=delta;
			_lockVelocity = false;
		}

		/// <summary>
		/// 设置目标速度
		/// </summary>
		/// <param name="target">目标速度</param>
		public void SpeedTo(float target){
			_lockVelocity = false;
			_targetVelocity = target;
		}

		/// <summary>
		/// 保留目标速度，把当前速度设置为0，并锁定。
		/// </summary>
		public void Stop () {
			SetCurrentVelocity (0, true);
		}

		/// <summary>
		/// 使当前速度值可以被改变
		/// </summary>
		public void UnLock () {
			_lockVelocity = false;
		}

		/// <summary>
		/// 使当前速度值不可被改变
		/// </summary>
		public void Lock () {
			_lockVelocity = false;
		}
	}

}