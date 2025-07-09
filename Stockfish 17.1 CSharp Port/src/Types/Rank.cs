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

namespace Stockfish_7._1_CSharp_Port.Types;

public struct Rank: IEquatable<Rank>
{
    public static readonly Rank RANK_1 = 0;
    public static readonly Rank RANK_2 = 1;
    public static readonly Rank RANK_3 = 2;
    public static readonly Rank RANK_4 = 3;
    public static readonly Rank RANK_5 = 4;
    public static readonly Rank RANK_6 = 5;
    public static readonly Rank RANK_7 = 6;
    public static readonly Rank RANK_8 = 7;
    public static readonly Rank RANK_NB = 8;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rank(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Rank other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Rank other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rank(Int32 value) => new Rank(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Rank r) => r.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Rank r) => r!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Rank r) => r==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rank relative_rank(Color c, Rank r) => (r ^ (c * 7)); 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rank relative_rank(Color c, Square s) => Rank.relative_rank(c, Square.rank_of(s)); 
}