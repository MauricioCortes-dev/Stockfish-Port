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

public class Misc
{
    // Version number or dev.
    public const String version = "17.1";

    // Returns the full name of the current Stockfish version.
    //
    // For local dev compiles we try to append the commit SHA and
    // commit date from git. If that fails only the local compilation
    // date is set and "nogit" is specified:
    //      Stockfish dev-YYYYMMDD-SHA
    //      or
    //      Stockfish dev-YYYYMMDD-nogit
    //
    // For releases (non-dev builds) we only include the version number:
    //      Stockfish version
    public static String engine_version_info()
    {
        //TODO - Falta implementacion completa y precisa
        return "Stockfish " + version;
    }
    
    public static String engine_info(bool to_uci) {
        return engine_version_info() + (to_uci ? "\nid author " : " by ")
                                     + "the Stockfish developers (see AUTHORS file)";
    }
    
    // Returns a string trying to describe the compiler we use
    public static String compiler_info()
    {
        //TODO - Falta implementacion completa y precisa
        return "compiler_info: NO INFO";
    }
}

// xorshift64star Pseudo-Random Number Generator
// This class is based on original code written and dedicated
// to the public domain by Sebastiano Vigna (2014).
// It has the following characteristics:
//
//  -  Outputs 64-bit numbers
//  -  Passes Dieharder and SmallCrush test batteries
//  -  Does not require warm-up, no zeroland to escape
//  -  Internal state is a single 64-bit integer
//  -  Period is 2^64 - 1
//  -  Speed: 1.60 ns/call (Core i7 @3.40GHz)
//
// For further analysis see
//   <http://vigna.di.unimi.it/ftp/papers/xorshift.pdf>
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