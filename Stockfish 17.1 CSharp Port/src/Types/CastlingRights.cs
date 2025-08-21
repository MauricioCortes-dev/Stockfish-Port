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

public struct CastlingRights: IEquatable<CastlingRights>
{
    public static readonly CastlingRights NO_CASTLING = 0;
    public static readonly CastlingRights WHITE_OO = 1;
    public static readonly CastlingRights WHITE_OOO = WHITE_OO << 1;
    public static readonly CastlingRights BLACK_OO = WHITE_OO << 2;
    public static readonly CastlingRights BLACK_OOO = WHITE_OO << 3;
    public static readonly CastlingRights KING_SIDE = WHITE_OO | BLACK_OO;
    public static readonly CastlingRights QUEEN_SIDE = WHITE_OOO | BLACK_OOO;
    public static readonly CastlingRights WHITE_CASTLING = WHITE_OO | WHITE_OOO;
    public static readonly CastlingRights BLACK_CASTLING = BLACK_OO | BLACK_OOO;
    public static readonly CastlingRights ANY_CASTLING = WHITE_CASTLING | BLACK_CASTLING;
    public static readonly CastlingRights CASTLING_RIGHT_NB = 16;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CastlingRights(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(CastlingRights other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is CastlingRights other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CastlingRights(Int32 value) => new CastlingRights(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(CastlingRights c) => c.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CastlingRights operator &(Color c, CastlingRights cr) =>(c == Color.WHITE ? WHITE_CASTLING : BLACK_CASTLING) & cr;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(CastlingRights c) => c!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(CastlingRights c) => c==0;
}