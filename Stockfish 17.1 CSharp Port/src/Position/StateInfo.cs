/*
  Stockfish, a UCI chess playing engine derived from Glaurung 2.1
  Copyright (C) 2004-2025 The Stockfish developers (see AUTHORS file)

  Stockfish is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Stockfish is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Stockfish_17_1_CSharp_Port.Types;

namespace Stockfish_17_1_CSharp_Port.Position;

using Key = System.UInt64;

// StateInfo struct stores information needed to restore a Position object to
// its previous state when we retract a move. Whenever a move is made on the
// board (by calling Position::do_move), a StateInfo object must be passed.
public class StateInfo
{
    // Copied when making a move
    Key materialKey;
    Key pawnKey;
    Key minorPieceKey;
    Key[] nonPawnKey = new Key[Color.COLOR_NB];
    Value[] nonPawnMaterial = new Value[Color.COLOR_NB];
    int    castlingRights;
    int    rule50;
    int    pliesFromNull;
    Square epSquare;
    
    // Not copied when making a move (will be recomputed anyhow)
    Key key;
    Bitboard checkersBB;
    StateInfo previous;
    StateInfo next;
    Bitboard[] blockersForKing  = new Bitboard[Color.COLOR_NB];
    Bitboard[] pinners = new Bitboard[Color.COLOR_NB];
    Bitboard[] checkSquares = new Bitboard[PieceType.PIECE_TYPE_NB];
    Piece capturedPiece;
    int repetition;
}