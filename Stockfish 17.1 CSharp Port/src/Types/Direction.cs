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

public struct Direction: IEquatable<Direction>
{
    public static readonly Direction NORTH = 8;
    public static readonly Direction EAST = 1;
    public static readonly Direction SOUTH = -NORTH;
    public static readonly Direction WEST = -EAST;
    public static readonly Direction NORTH_EAST = NORTH + EAST;
    public static readonly Direction SOUTH_EAST = SOUTH + EAST;
    public static readonly Direction SOUTH_WEST = SOUTH + WEST;
    public static readonly Direction NORTH_WEST = NORTH + WEST;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Direction(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Direction other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Direction other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Direction(Int32 value) => new Direction(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Direction d) => d.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Direction operator +(Direction d1, Direction d2) => d1.value + d2.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Direction operator *(Int32 i, Direction d) =>i * d.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Direction d) => d!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Direction d) => d==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Direction pawn_push(Color c) => c == Color.WHITE ? NORTH : SOUTH; 
    
}