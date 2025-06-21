$.onUpdate(_ => {
  let lastPlayers = $.state.players ?? [];
  let players = $.getPlayersNear($.getPosition(), Infinity);
  for (let player of players) {
    if (lastPlayers.every(p => p.id !== player.id)) {
      $.setPlayerScript(player);
    }
  }
  $.state.players = players;
});
