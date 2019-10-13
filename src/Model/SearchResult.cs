using System;

public class SearchResult
	{
	private Enemy_present _Pres;
	private Ship _Ship;
	private int _Row;
	private int _Col;

	public Enemy_present Presence { 
		get { return _Pres; }
	}

	public Ship Ship { 
		get { return _Ship; } 
	}

	public int Row { 
		get { return _Row; }
	}

	public int Column { 
		get { return _Col; }
	}
	public SearchResult (Enemy_present pres,int row, int col)
	{
		_Pres = pres;
		_Row = row;
		_Col = col;
		_Ship = null;
	}

	public SearchResult (Enemy_present pres, Ship ship, int row, int col) :this(pres, row ,col) {
		_Ship = ship;
	}
}

