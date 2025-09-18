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


namespace Stockfish_17_1_CSharp_Port.MoveGen;

using System.Runtime.CompilerServices;

public struct GenType
{
  public static readonly GenType CAPTURES = 0;
  public static readonly GenType QUIETS = 1;
  public static readonly GenType EVASIONS = 2;
  public static readonly GenType NON_EVASIONS = 3;
  public static readonly GenType LEGAL = 4;
  
  public readonly Int32 value;
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public GenType(Int32 value) => this.value = value;
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(GenType other) => value == other.value;
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override bool Equals(object? obj) => obj is GenType other && Equals(other);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override int GetHashCode() => value.GetHashCode();
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator GenType(Int32 value) => new GenType(value);
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator Int32(GenType b) => b.value;
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator true(GenType b) => b!=0;
    
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator false(GenType b) => b==0;
  
}