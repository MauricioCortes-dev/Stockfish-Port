﻿/*
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
using System.Diagnostics;

namespace Stockfish_7._1_CSharp_Port.Types;

public struct Color: IEquatable<Color>
{
    public static readonly Color WHITE = 0;
    public static readonly Color BLACK = 1;
    public static readonly Color COLOR_NB = 2;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Color other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(Int32 value) => new Color(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Color c) => c.value;
    
    // Toggle color
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator ~(Color color) =>  color ^ BLACK;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Color c) => c!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Color c) => c==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color color_of(Piece pc) {
        Debug.Assert(pc != Piece.NO_PIECE);
        return pc >> 3;
    }
}