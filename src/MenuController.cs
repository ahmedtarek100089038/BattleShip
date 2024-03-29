using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The menu controller handles the drawing and user interactions
/// from the menus in the game. These include the main menu, game
/// menu and the settings m,enu.
/// </summary>

static class MenuController
{

	/// <summary>
	/// The menu structure for the game.
	/// </summary>
	/// <remarks>
	/// These are the text captions for the menu items.
	/// </remarks>
	private static readonly string[][] _menuStructure = {
		new string[] {
			"PLAY",
			"LEVEL",
			"SCORES",
			"MUTE",
			"QUIT"
		},
		new string[] {
			"BACK",
			"SURRENDER",
			"QUIT GAME"
		},
		new string[] {
			"BACK",
			"MAIN MENU",
			"QUIT GAME"
		},
		new string[] {
			"EASY",
			"MEDIUM",
			"HARD"
		}

	};
	/// <summary>
	/// Change button sizes to accommodate font size change
	/// author: B'Jorn Sterling
	/// </summary>
	private const int MENU_TOP = 550;
	private const int MENU_LEFT = 100;
	private const int MENU_GAP = 5;
	private const int BUTTON_WIDTH = 100;
	private const int BUTTON_HEIGHT = 20;
	private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;

	private const int TEXT_OFFSET = 0;
	private const int MAIN_MENU = 0;
	private const int GAME_MENU = 1;
	private const int DEPLOY_MENU = 2;
	private const int LEVEL_MENU = 3;

	private const int MAIN_MENU_PLAY_BUTTON = 0;
	private const int MAIN_MENU_LEVEL_BUTTON = 1;
	private const int MAIN_MENU_TOP_SCORES_BUTTON = 2;
	private const int MAIN_MENU_MUTE_BUTTON = 3;
	private const int MAIN_MENU_QUIT_BUTTON = 4;

	private const int LEVEL_MENU_EASY_BUTTON = 0;
	private const int LEVEL_MENU_MEDIUM_BUTTON = 1;
	private const int LEVEL_MENU_HARD_BUTTON = 2;

	private const int LEVEL_MENU_EXIT_BUTTON = 3;
	private const int GAME_MENU_BACK_BUTTON = 0;
	private const int GAME_MENU_SURRENDER_BUTTON = 1;
	private const int GAME_MENU_QUIT_BUTTON = 2;

	/// <summary>
	/// Edited by: Eva
	/// Adding new game menu in the deploy section
	/// </summary>'
	private const int DEPLOY_MENU_BACK_BUTTON = 0;
	private const int DEPLOY_MENU_MM_BUTTON = 1;
	private const int GAME_MENU_MUTE_BUTTON = 2;
	private const int DEPLOY_MENU_QUIT_BUTTON = 3;

	private static bool isMute = false;
	private static int bgmPlaying = 1;

	private static readonly Color MENU_COLOR = SwinGame.RGBAColor(2, 255, 255, 255);

	private static readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);
	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleMainMenuInput()
	{
		HandleMenuInput(MAIN_MENU, 0, 0);
	}

	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleSetupMenuInput()
	{
		bool handled = false;
		handled = HandleMenuInput(LEVEL_MENU, 1, 1);

		if (!handled) {
			HandleMenuInput(MAIN_MENU, 0, 0);
		}
	}

	/// <summary>
	/// Handle input in the game menu.
	/// </summary>
	/// <remarks>
	/// Player can return to the game, surrender, or quit entirely
	/// </remarks>
	public static void HandleGameMenuInput()
	{
		HandleMenuInput(GAME_MENU, 0, 0);
	}

	///Edited by: Eva
	/// Player can returns to game, return to main menu, or quit entirely
	public static void HandleDeployMenuInput ()
	{
		HandleMenuInput (DEPLOY_MENU, 0, 0);
	}

	/// <summary>
	/// Handles input for the specified menu.
	/// </summary>
	/// <param name="menu">the identifier of the menu being processed</param>
	/// <param name="level">the vertical level of the menu</param>
	/// <param name="xOffset">the xoffset of the menu</param>
	/// <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
	private static bool HandleMenuInput(int menu, int level, int xOffset)
	{
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
			GameController.EndCurrentState();
			return true;
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			int i = 0;
			for (i = 0; i <= _menuStructure[menu].Length - 1; i++) {
				//IsMouseOver the i'th button of the menu
				if (IsMouseOverMenu(i, level, xOffset)) {
					PerformMenuAction(menu, i);
					return true;
				}
			}

			if (level > 0) {
				//none clicked - so end this sub menu
				GameController.EndCurrentState();
			}
		}

		return false;
	}

	/// <summary>
	/// Draws the main menu to the screen.
	/// </summary>
	public static void DrawMainMenu()
	{
		SwinGame.DrawText ("Hotkeys", Color.Orange, GameResources.GameFont ("score"), 300, 280);
		SwinGame.DrawText ("F1 - Toggle fullscreen", Color.Orange, GameResources.GameFont ("score"), 175, 320);
		SwinGame.DrawText ("F2 - Take screenshot", Color.Orange, GameResources.GameFont ("score"), 175, 370);
		DrawButtons(MAIN_MENU);
	}

	/// <summary>
	/// Draws the Game menu to the screen
	/// </summary>
	public static void DrawGameMenu()
	{
		DrawButtons(GAME_MENU);
	}

	///Edited by: Eva
	/// Draws the Game menu (deploy section) to screen
	public static void DrawDeployMenu ()
	{
		
		DrawButtons(DEPLOY_MENU);
	}

	/// <summary>
	/// Draws the settings menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu
	/// </remarks>
	public static void DrawSettings()
	{
		//Clears the Screen to Black
		//SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons(MAIN_MENU);
		DrawButtons(LEVEL_MENU, 1, 1);
	}

	/// <summary>
	/// Draw the buttons associated with a top level menu.
	/// </summary>
	/// <param name="menu">the index of the menu to draw</param>
	private static void DrawButtons(int menu)
	{
		DrawButtons(menu, 0, 0);
	}

	/// <summary>
	/// Draws the menu at the indicated level.
	/// </summary>
	/// <param name="menu">the menu to draw</param>
	/// <param name="level">the level (height) of the menu</param>
	/// <param name="xOffset">the offset of the menu</param>
	/// <remarks>
	/// The menu text comes from the _menuStructure field. The level indicates the height
	/// of the menu, to enable sub menus. The xOffset repositions the menu horizontally
	/// to allow the submenus to be positioned correctly.
	/// </remarks>
	private static void DrawButtons (int menu, int level, int xOffset)
	{
		int btnTop = 0;

		btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int i = 0;
		string s = "UNMUTE";
		for (i = 0; i <= _menuStructure [menu].Length - 1; i++) {
			int btnLeft = 0;
			btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
			//SwinGame.FillRectangle(Color.White, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT)
			SwinGame.DrawTextLines (_menuStructure [menu] [i], MENU_COLOR, Color.Black, GameResources.GameFont ("Courier"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);
			if (_menuStructure [menu] [i] == "MUTE" && isMute == true){
				SwinGame.DrawTextLines (s, Color.Red, Color.Black, GameResources.GameFont ("Courier"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);
		}
			if (SwinGame.MouseDown (MouseButton.LeftButton) & IsMouseOverMenu (i, level, xOffset)) {
				SwinGame.DrawRectangle (HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}

	/// <summary>
	/// Determined if the mouse is over one of the button in the main menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <returns>true if the mouse is over that button</returns>
	private static bool IsMouseOverButton(int button)
	{
		return IsMouseOverMenu(button, 0, 0);
	}

	/// <summary>
	/// Checks if the mouse is over one of the buttons in a menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <param name="level">the level of the menu</param>
	/// <param name="xOffset">the xOffset of the menu</param>
	/// <returns>true if the mouse is over the button</returns>
	private static bool IsMouseOverMenu(int button, int level, int xOffset)
	{
		int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);

		return UtilityFunctions.IsMouseInRectangle(btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
	}

	/// <summary>
	/// A button has been clicked, perform the associated action.
	/// </summary>
	/// <param name="menu">the menu that has been clicked</param>
	/// <param name="button">the index of the button that was clicked</param>
	private static void PerformMenuAction(int menu, int button)
	{
		switch (menu) {
			case MAIN_MENU:
				PerformMainMenuAction(button);
				break;
			case LEVEL_MENU:
				PerformSetupMenuAction(button);
				break;
			case GAME_MENU:
				PerformGameMenuAction(button);
				break;
			case DEPLOY_MENU:
				PerformDeployMenuAction (button);
				break;
		}
	}

	/// <summary>
	/// The main menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformMainMenuAction(int button)
	{
		switch (button) {
			case MAIN_MENU_PLAY_BUTTON:
				GameController.StartGame();
				break;
			case MAIN_MENU_LEVEL_BUTTON:
				GameController.AddNewState(GameState.AlteringSettings);
				break;
			case MAIN_MENU_TOP_SCORES_BUTTON:
				GameController.AddNewState(GameState.ViewingHighScores);
				break;
			case MAIN_MENU_MUTE_BUTTON:
				Mute ();
				break;
			case MAIN_MENU_QUIT_BUTTON:
				GameController.EndCurrentState();
				break;
		}
	}

	/// <summary>
	/// The setup menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformSetupMenuAction(int button)
	{
		switch (button) {
			case LEVEL_MENU_EASY_BUTTON:
				GameController.SetDifficulty(AIOption.Easy);
				break;
			case LEVEL_MENU_MEDIUM_BUTTON:
				GameController.SetDifficulty(AIOption.Medium);
				break;
			case LEVEL_MENU_HARD_BUTTON:
				GameController.SetDifficulty(AIOption.Hard);
				break;
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState();
	}

	/// <summary>
	/// The game menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformGameMenuAction(int button)
	{
		switch (button) {
			case GAME_MENU_BACK_BUTTON:
				GameController.EndCurrentState();
				break;
			case GAME_MENU_SURRENDER_BUTTON:
				GameController.EndCurrentState();
				//end game menu
				GameController.EndCurrentState();
				//end game
				break;
			case GAME_MENU_QUIT_BUTTON:
				GameController.AddNewState(GameState.Quitting);
				break;
		}
	}

	/// <summary>
	/// Edited by: Eva 
	/// The game menu was clicke, perform the button's action. 
	/// </summary>

	private static void PerformDeployMenuAction (int button)
	{
		switch (button) {
		case DEPLOY_MENU_BACK_BUTTON:
			GameController.EndCurrentState ();
			break;
		case DEPLOY_MENU_MM_BUTTON:
			GameController.EndCurrentState ();
			//end game menu
			GameController.EndCurrentState ();
			//end game
			break;
		case GAME_MENU_MUTE_BUTTON:
			Mute ();
			break;
		case DEPLOY_MENU_QUIT_BUTTON:
			GameController.AddNewState (GameState.Quitting);
			break;
		}
	}

	private static void Mute ()
	{
		if (isMute == false) {
			Audio.CloseAudio ();
			isMute = true;
		} else {
			Audio.OpenAudio ();
			switch (bgmPlaying) {
			case 1:
				SwinGame.PlayMusic (GameResources.GameMusic ("Background"));
				isMute = false;
				break;
	
			}

			}
		}
	}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
