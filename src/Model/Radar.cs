using System;
using SwinGameSDK;

public static class Radar
	{
		public Radar ()
		{
		}

	/// <summary>
	/// Search the location that the mouse if over.
	/// </summary>
	private static void Search ()
	{
		Point2D mouse = default (Point2D);

		mouse = SwinGame.MousePosition ();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32 (Math.Floor ((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32 (Math.Floor ((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack (row, col);
			}
		}
	}
}
	

