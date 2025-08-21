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
using System.Runtime.CompilerServices;

namespace Stockfish_17_1_CSharp_Port.Types;

public struct Piece: IEquatable<Piece>
{
    public static readonly Piece NO_PIECE = 0;
    public static readonly Piece W_PAWN = PieceType.PAWN;
    public static readonly Piece W_KNIGHT = 2;
    public static readonly Piece W_BISHOP = 3;
    public static readonly Piece W_ROOK = 4;
    public static readonly Piece W_QUEEN = 5;
    public static readonly Piece W_KING = 6;
    public static readonly Piece B_PAWN = PieceType.PAWN + 8;
    public static readonly Piece B_KNIGHT = 10;
    public static readonly Piece B_BISHOP = 11;
    public static readonly Piece B_ROOK = 12;
    public static readonly Piece B_QUEEN = 13;
    public static readonly Piece B_KING = 14;
    public static readonly Piece PIECE_NB = 16;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Piece other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Piece other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Piece(Int32 value) => new Piece(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Piece p) => p.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Piece(PieceType p) => new Piece(p.value) ;
    
    // Swap color of piece B_KNIGHT <-> W_KNIGHT
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Piece operator ~(Piece pc) => pc ^ 8;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Piece p) => p!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Piece p) => p==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Piece make_piece(Color c, PieceType pt) =>(c << 3) + pt; 
}