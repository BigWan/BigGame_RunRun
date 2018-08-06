using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 接壤方向
	/// </summary>
	public enum JoinDirection {
		Straight  = 0,  // 直线
		Left = 1,       // 左拐
		Right = 2       // 右拐
	}

	public enum TwordDirection{
		forward = 0,
		right =1,
		back = 2,
		left = 3
	}

	/// <summary>
	/// 跑道插头
	/// </summary>
	public abstract class Plug : MonoBehaviour {

		/// <summary>
		/// Plug的朝向，直线，左，右
		/// </summary>
		public JoinDirection joinDirection;

		/// <summary>
		/// 可以在plug处连接的Block集合
		/// </summary>
		public List<Block> joinableBlocks;

		/// <summary>
		/// 当前块的前进方向
		/// </summary>
		public TwordDirection toward;


		/// <summary>
		/// 生成下一个块
		/// </summary>
		public abstract void SpawnNext();




	}
}