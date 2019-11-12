using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

public class Mines
	{
		private int x_pos, y_pos;
		protected static Random _Random = new Random ();
		private bool hit = false;

		public Mines ()
		{
			X = _Random.Next (0, 11);
			Y = _Random.Next (0, 11);
		}

		public int X { 
			get { return x_pos;}
			set { x_pos = value;}
		}

		public int Y { 
			get { return y_pos;}
			set { y_pos = value;}
		}
	public bool Hit { 
		get { return hit;}
		set { hit = value;}
	}

}

