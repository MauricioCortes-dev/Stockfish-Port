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

public struct Depth: IEquatable<Depth>
{
    // The following DEPTH_ constants are used for transposition table entries
    // and quiescence search move generation stages. In regular search, the
    // depth stored in the transposition table is literal: the search depth
    // (effort) used to make the corresponding transposition table value. In
    // quiescence search, however, the transposition table entries only store
    // the current quiescence move generation stage (which should thus compare
    // lower than any regular search depth).
    public static readonly Depth DEPTH_QS = 0;
    // For transposition table entries where no searching at all was done
    // (whether regular or qsearch) we use DEPTH_UNSEARCHED, which should thus
    // compare lower than any quiescence or regular depth. DEPTH_ENTRY_OFFSET
    // is used only for the transposition table entry occupancy check (see tt.cpp),
    // and should thus be lower than DEPTH_UNSEARCHED.
    public static readonly Depth DEPTH_UNSEARCHED = -2;
    public static readonly Depth DEPTH_ENTRY_OFFSET = -3;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Depth(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Depth other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Depth other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Depth(Int32 value) => new Depth(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Depth d) => d.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Depth d) => d!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Depth d) => d==0;
}