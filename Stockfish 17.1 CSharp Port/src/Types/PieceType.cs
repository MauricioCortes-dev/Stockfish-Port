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

public struct PieceType: IEquatable<PieceType>
{
    public static readonly PieceType NO_PIECE_TYPE = 0;
    public static readonly PieceType PAWN = 1;
    public static readonly PieceType KNIGHT = 2;
    public static readonly PieceType BISHOP = 3;
    public static readonly PieceType ROOK = 4;
    public static readonly PieceType QUEEN = 5;
    public static readonly PieceType KING = 6;
    public static readonly PieceType ALL_PIECES = 0;
    public static readonly PieceType PIECE_TYPE_NB = 8;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PieceType(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(PieceType other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is PieceType other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator PieceType(Int32 value) => new PieceType(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(PieceType p) => p.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(PieceType p) => p!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(PieceType p) => p==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PieceType type_of(Piece pc) => pc & 7; 
}