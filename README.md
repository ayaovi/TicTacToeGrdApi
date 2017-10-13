# TicTacToeGrd

[![Build status](https://ci.appveyor.com/api/projects/status/ipv5c5y04mauw3wy?svg=true)](https://ci.appveyor.com/project/ayaovi/tictactoegrd)
[![Codecov branch](https://img.shields.io/codecov/c/github/ayaovi/TicTacToeGrd/master.svg)](https://codecov.io/gh/ayaovi/TicTacToeGrd)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/ayaovi/TicTacToeGrd/blob/master/LICENSE)

## Overview

TicTacToe Grand is the normal TicTacToe on a bigger scale. It is made of 3x3 grids of 3x3 cells (i.e. the game board is composed of 9x9 cells). In my head (at least for now), the game can played in the following ways:

- Player vs Player (PvP),
- Player vs AI (PvA)
- AI vs AI (AvA)

### PvP

This is a one that would give rewards (i.e. points, coins etc...).

### PvA

This can be used as training exercise for an AgniKai

### AvA

Cannot really see a concrete need for this type just yet. But as far as I can tell, this can be used to learn some new moves or strategy.

## TODO

- The AI still send the opponent to already won grid. Fix this.
- Re-look at cells' probability update (as it seems to be working poorly).
- Tighten up the grip on player winning (i.e. do not make it too easy for the player).
- Handle token clearing after expiration (or should we have tokens in the first place).
- A player in the "Active Players" list should be clickable. Upon click, one could see the player details or challenge him/her.
- Implement PvP logic, on both server and front end side.
- Move back-end web api to .Net Core.

## IDEAS

- Add audience/observer feature, whereby, other users can connect just to watch other people play.