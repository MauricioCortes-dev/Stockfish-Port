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
namespace Stockfish_7._1_CSharp_Port;

// Magic holds all magic bitboards relevant data for a single square
public struct Magic
{
    public Bitboard  mask;
    public Bitboard[] attacks;
    public Bitboard magic;
    public Int32 shift;
    public Int32 offset;
    
    // Compute the attack's index using the 'magic bitboards' approach
    public UInt32 index(Bitboard occupied) 
    {
        return (UInt32)(((occupied & mask) * magic) >> shift);
    }
    
    public Bitboard attacks_bb(Bitboard occupied)=> attacks[offset + index(occupied)];
}