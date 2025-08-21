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

// Value is used as an alias for int, this is done to differentiate between a search
// value and any other integer value. The values used in search are always supposed
// to be in the range (-VALUE_NONE, VALUE_NONE] and should not exceed this range.
public struct Value: IEquatable<Value>
{
    public static readonly Value VALUE_ZERO     = 0;
    public static readonly Value VALUE_DRAW     = 0;
    public static readonly Value VALUE_NONE     = 32002;
    public static readonly Value VALUE_INFINITE = 32001;
    
    public static readonly Value VALUE_MATE             = 32000;
    public static readonly Value VALUE_MATE_IN_MAX_PLY  = VALUE_MATE - Types.MAX_PLY;
    public static readonly Value VALUE_MATED_IN_MAX_PLY = -VALUE_MATE_IN_MAX_PLY;
    
    public static readonly Value VALUE_TB                 = VALUE_MATE_IN_MAX_PLY - 1;
    public static readonly Value VALUE_TB_WIN_IN_MAX_PLY  = VALUE_TB - Types.MAX_PLY;
    public static readonly Value VALUE_TB_LOSS_IN_MAX_PLY = -VALUE_TB_WIN_IN_MAX_PLY;
    
    // In the code, we make the assumption that these values
    // are such that non_pawn_material() can be used to uniquely
    // identify the material on the board.
    public static readonly Value PawnValue   = 208;
    public static readonly Value KnightValue = 781;
    public static readonly Value BishopValue = 825;
    public static readonly Value RookValue   = 1276;
    public static readonly Value QueenValue  = 2538;
    
    public static readonly Value[] PieceValue = 
    { 
        VALUE_ZERO, PawnValue, KnightValue, BishopValue, RookValue, QueenValue, VALUE_ZERO, VALUE_ZERO,
        VALUE_ZERO, PawnValue, KnightValue, BishopValue, RookValue, QueenValue, VALUE_ZERO, VALUE_ZERO
    };
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Value(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Value other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Value other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Value(Int32 value) => new Value(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Value v) => v.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Value v) => v!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Value v) => v==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool is_valid(Value value) => value != VALUE_NONE; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool is_win(Value value) {
        Debug.Assert(is_valid(value));
        return value >= VALUE_TB_WIN_IN_MAX_PLY;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool is_loss(Value value) {
        Debug.Assert(is_valid(value));
        return value <= VALUE_TB_LOSS_IN_MAX_PLY;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool is_decisive(Value value) =>is_win(value) || is_loss(value); 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static  Value mate_in(int ply) => VALUE_MATE - ply; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static  Value mated_in(int ply) => -VALUE_MATE + ply; 
    
}