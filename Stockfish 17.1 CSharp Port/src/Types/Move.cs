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

namespace Stockfish_17_1_CSharp_Port.Types;

// A move needs 16 bits to be stored
//
// bit  0- 5: destination square (from 0 to 63)
// bit  6-11: origin square (from 0 to 63)
// bit 12-13: promotion piece type - 2 (from KNIGHT-2 to QUEEN-2)
// bit 14-15: special move flag: promotion (1), en passant (2), castling (3)
// NOTE: en passant bit is set only when a pawn can be captured
//
// Special cases are Move::none() and Move::null(). We can sneak these in because
// in any normal move the destination square and origin square are always different,
// but Move::none() and Move::null() have the same origin and destination square.
public class Move
{
    public static readonly Move null_move =  new Move(65);
    public static readonly Move none_move = new Move(0); 
    
    protected readonly UInt16 data;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Move(UInt16 d) =>this.data = d;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Move(Square from, Square to) => this.data = (UInt16)((from << 6) + to);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator UInt16(Move m) => m.data;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Move m) => m != 0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Move m) => m == 0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Move m1, Move m2) => m1.data == m2.data;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Move m1, Move m2) => m1.data != m2.data;
    
    protected bool Equals(Move other)
    {
        return data == other.data;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Move)obj);
    }

    public override int GetHashCode()
    {
        return data.GetHashCode();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static Move make(Square from, Square to, MoveType T, PieceType pt) => new Move((UInt16)(T + ((pt - PieceType.KNIGHT) << 12) + (from << 6) + to));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Move make(Square from, Square to, MoveType T) => Move.make(from, to, T, PieceType.KNIGHT);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Move make(Square from, Square to) => Move.make(from, to, MoveType.NORMAL, PieceType.KNIGHT);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool is_ok() => none_move.data != data && null_move.data != data; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square from_sq() {
        Debug.Assert(is_ok());
        return (data >> 6) & 0x3F;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square to_sq() {
        Debug.Assert(is_ok());
        return data & 0x3F;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int from_to() => data & 0xFFF; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MoveType type_of() => data & (3 << 14); 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PieceType promotion_type() =>((data >> 12) & 3) + PieceType.KNIGHT; 

    // Based on a congruential pseudo-random number generator
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UInt64 MoveHash() => data * 6364136223846793005UL + 1442695040888963407UL;
}