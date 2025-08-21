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

public struct Square: IEquatable<Square>
{
    public static readonly Square SQ_A1 = 0; public static readonly Square SQ_B1 = 1; public static readonly Square SQ_C1 = 2; public static readonly Square SQ_D1 = 3; public static readonly Square SQ_E1 = 4; public static readonly Square SQ_F1 = 5; public static readonly Square SQ_G1 = 6; public static readonly Square SQ_H1 = 7;
    public static readonly Square SQ_A2 = 8; public static readonly Square SQ_B2 = 9; public static readonly Square SQ_C2 = 10; public static readonly Square SQ_D2 = 11; public static readonly Square SQ_E2 = 12; public static readonly Square SQ_F2 = 13; public static readonly Square SQ_G2 = 14; public static readonly Square SQ_H2 = 15;
    public static readonly Square SQ_A3 = 16; public static readonly Square SQ_B3 = 17; public static readonly Square SQ_C3 = 18; public static readonly Square SQ_D3 = 19; public static readonly Square SQ_E3 = 20; public static readonly Square SQ_F3 = 21; public static readonly Square SQ_G3 = 22; public static readonly Square SQ_H3 = 23;
    public static readonly Square SQ_A4 = 24; public static readonly Square SQ_B4 = 25; public static readonly Square SQ_C4 = 26; public static readonly Square SQ_D4 = 27; public static readonly Square SQ_E4 = 28; public static readonly Square SQ_F4 = 29; public static readonly Square SQ_G4 = 30; public static readonly Square SQ_H4 = 31;
    public static readonly Square SQ_A5 = 32; public static readonly Square SQ_B5 = 33; public static readonly Square SQ_C5 = 34; public static readonly Square SQ_D5 = 35; public static readonly Square SQ_E5 = 36; public static readonly Square SQ_F5 = 37; public static readonly Square SQ_G5 = 38; public static readonly Square SQ_H5 = 39;
    public static readonly Square SQ_A6 = 40; public static readonly Square SQ_B6 = 41; public static readonly Square SQ_C6 = 42; public static readonly Square SQ_D6 = 43; public static readonly Square SQ_E6 = 44; public static readonly Square SQ_F6 = 45; public static readonly Square SQ_G6 = 46; public static readonly Square SQ_H6 = 47;
    public static readonly Square SQ_A7 = 48; public static readonly Square SQ_B7 = 49; public static readonly Square SQ_C7 = 50; public static readonly Square SQ_D7 = 51; public static readonly Square SQ_E7 = 52; public static readonly Square SQ_F7 = 53; public static readonly Square SQ_G7 = 54; public static readonly Square SQ_H7 = 55;
    public static readonly Square SQ_A8 = 56; public static readonly Square SQ_B8 = 57; public static readonly Square SQ_C8 = 58; public static readonly Square SQ_D8 = 59; public static readonly Square SQ_E8 = 60; public static readonly Square SQ_F8 = 61; public static readonly Square SQ_G8 = 62; public static readonly Square SQ_H8 = 63;
    public static readonly Square SQ_NONE = 64;
    
    public static readonly Square SQUARE_ZERO = 0;
    public static readonly Square SQUARE_NB = 64;
    
    public readonly Int32 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square(Int32 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Square other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Square other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Square(Int32 value) => new Square(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Int32(Square s) => s.value;
    
    // Additional operators to add a Direction to a Square
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square operator +(Square s, Direction d) => s.value + d.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square operator -(Square s, Direction d) => s.value - d.value;
    
    //From bitboard.h
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator |(Square s1, Square s2) => Bitboard.square_bb(s1) | s2;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Square b) => b!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Square b) => b==0;
    
    // Swap A1 <-> A8
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square flip_rank(Square s) => s ^ SQ_A8;
    
    // Swap A1 <-> H1
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square flip_file(Square s) => s ^ SQ_H1;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square make_square(File f, Rank r) => (r << 3) + f; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool is_ok(Square s) => s >= SQ_A1 && s <= SQ_H8; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static File file_of(Square s) =>s & 7; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rank rank_of(Square s) => s >> 3; 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square relative_square(Color c, Square s) => s ^ (c * 56); 
    
}