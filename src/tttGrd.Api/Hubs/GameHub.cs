using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using tttGrd.Api.Models;
using tttGrd.Api.Persistence;

namespace tttGrd.Api.Hubs
{
  [HubName("gameHub")]
  public class GameHub : Hub
  {
    private readonly IDatabaseRepository _database;

    public GameHub(IDatabaseRepository database)
    {
      _database = database;
    }

    public async Task AnnounceAsync(string username)
    {
      //await _database.AddPlayerAsync(username);
      await _database.AddConnectionAsync(username, Context.ConnectionId);
    }

    public async Task NotifyPlayerAsync(string challengeeId, string challengerId)
    {
      var connId = await _database.GetConnectionAsync(challengeeId);
      Clients.Client(connId).notifyOfChallenge(challengerId);
    }

    public async Task NotifyOfChallengeAcceptedAsync(string challengeeId, string challengerId)
    {
      var connId = await _database.GetConnectionAsync(challengerId);
      Clients.Client(connId).notifyOfChallengeAccpeted(challengeeId);
    }

    public async Task AgniKaiStartNotification(string challengeeId, string challengerId)
    {
      var connId = await _database.GetConnectionAsync(challengerId);
      await Clients.Client(connId).notifyOfChallengeAccpeted(challengeeId);
    }

    public async Task JoinAgniKai(string agniKaiTicket)
    {
      await Groups.Add(Context.ConnectionId, agniKaiTicket);
    }

    public Task LeaveAgniKai(string agniKaiTicket)
    {
      return Groups.Remove(Context.ConnectionId, agniKaiTicket);
    }

    // ReSharper disable once InconsistentNaming
    public async Task SendMoveAI(string agniKaiTicket, int grid, int cell, char tile)
    {
      Field IndicatorFromTile(char t)
      {
        switch (t)
        {
          case 'x':
            return Field.X;
          case 'o':
            return Field.O;
          default:
            return Field.Empty;
        }
      }
      var playerIndicator = IndicatorFromTile(tile);
      var aiIndicator = playerIndicator == Field.X ? Field.O : Field.X;
      var playerMove = (grid, cell);
      await _database.RecordMoveAsync(agniKaiTicket, playerMove, playerIndicator);
      var state = await _database.GetStateAsync(agniKaiTicket);

      var agnikai = await _database.GetAgniKaiByTicketAsync(agniKaiTicket);
      var ai = agnikai.GetGamerByIndicator(aiIndicator) as AI;
      ai.GameState = state;
      // ReSharper disable once PossibleNullReferenceException
      ai.CellProbabilities = Program.UpdateCellsProbabilities(ai.CellProbabilities, state, playerMove, aiIndicator);
      var aiMove = ai.MakeProbabilityBasedMove(playerMove);
      await _database.RecordMoveAsync(agniKaiTicket, aiMove, aiIndicator);
      state = await _database.GetStateAsync(agniKaiTicket);
      ai.GameState = state;
      ai.CellProbabilities = Program.UpdateCellsProbabilities(ai.CellProbabilities, state, aiMove, aiIndicator);

      Clients.Group(agniKaiTicket).broadcastState(state); /* we might want to encrypt the state being published and save it with a time stamp. */
    }
  }
}