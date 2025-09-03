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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Stockfish_17_1_CSharp_Port.Types;

namespace Stockfish_17_1_CSharp_Port.Position;

// Position class stores information regarding the board representation as
// pieces, side to move, hash keys, castling info, etc. Important methods are
// do_move() and undo_move(), used by the search to update node info when
// traversing the search tree.
public class Position : ICloneable 
{
    // Data members
    public Piece[] board = new Piece[Square.SQUARE_NB];
    public Bitboard[] byTypeBB = new Bitboard[PieceType.PIECE_TYPE_NB];
    public Bitboard[] byColorBB = new Bitboard[Color.COLOR_NB];
    public int[] pieceCount = new int[Piece.PIECE_NB];
    public int[] castlingRightsMask = new int[Square.SQUARE_NB];
    public Square[] castlingRookSquare = new Square[CastlingRights.CASTLING_RIGHT_NB];
    public Bitboard[] castlingPath = new Bitboard[CastlingRights.CASTLING_RIGHT_NB];
    public StateInfo st;
    public int gamePly;
    public Color sideToMove;
    public bool chess960;
        
    public static void init()
    {
        
    }
    
    public object Clone() => throw new NotSupportedException("Position no es clonable/copiable");

    // FEN string input/output
    public Position set(String fenStr, bool isChess960, StateInfo si){
        return this;
    }

    public Position set(String code, Color c, StateInfo si)
    {
        return this;
    }

    public String fen()
    {
        return "";
    }
    
    // Position representation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(params PieceType[] pts)
    {
        Bitboard result = 0;
        foreach (PieceType pt in pts)
        {
            result |= byTypeBB[pt];
        }
        
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces()
    {
        return pieces(PieceType.ALL_PIECES);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(Color c){ return byColorBB[c]; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(Color c, params PieceType[] pts)
    {
        return pieces(c) & pieces(pts);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece piece_on(Square s)
    {
        Debug.Assert(Square.is_ok(s));
        return board[s];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square ep_square() { return st.epSquare; }
    
    
}