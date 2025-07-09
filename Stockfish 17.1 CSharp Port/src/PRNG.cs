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
namespace Stockfish_7._1_CSharp_Port;

public class PRNG
{
    UInt64 s;

    UInt64 rand64()
    {
        s ^= s >> 12;
        s ^= s << 25; 
        s ^= s >> 27;
        return s * 2685821657736338717L;
    }

    public PRNG(int seed)
    {
        s = (UInt64)seed;
    }
    
    UInt64 rand() {
        return rand64();
    }
    
    public UInt64 sparse_rand() {
        return (rand64() & rand64() & rand64());
    }
}