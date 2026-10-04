<%@ Page Language="C#" AutoEventWireup="true" CodeFile="04-4thweek2026-live.aspx.cs" Inherits="_2026_04_4thweek2026_live" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="color-scheme" content="dark">
    <title>Week <%= WeekNumber %> Live -- Bell Football Pool</title>
    <link rel="stylesheet" href="https://use.typekit.net/oct3isk.css">
    <style>
        :root {
            --bg: #0a0e13;
            --surface: #121820;
            --raised: #1a222d;
            --line: #263243;
            --line-soft: #1c2532;
            --text: #e9eef4;
            --muted: #94a2b3;
            --faint: #5d6b7c;
            --win: #35d07f;
            --win-bg: rgba(53, 208, 127, .14);
            --loss: #ff6b6b;
            --loss-bg: rgba(255, 107, 107, .12);
            --live: #ffb020;
            --live-bg: rgba(255, 176, 32, .12);
            --turf: #143524;
            --turf-line: rgba(255, 255, 255, .13);
            --font: ardoise-narrow-std, "Segoe UI", system-ui, -apple-system, "Helvetica Neue", Arial, sans-serif;
            --font-tight: ardoise-compact-std, ardoise-narrow-std, "Segoe UI", system-ui, sans-serif;
        }

        * { box-sizing: border-box; }
        html { background: var(--bg); }
        body {
            margin: 0;
            background: var(--bg);
            color: var(--text);
            font-family: var(--font);
            font-size: 15px;
            line-height: 1.35;
            -webkit-font-smoothing: antialiased;
        }
        a { color: inherit; }
        .wrap { max-width: 1440px; margin: 0 auto; padding: 0 20px; }
        .num { font-variant-numeric: tabular-nums; }

        /* ---------- top bar ---------- */
        .top {
            position: sticky; top: 0; z-index: 30;
            background: rgba(10, 14, 19, .93);
            -webkit-backdrop-filter: blur(8px); backdrop-filter: blur(8px);
            border-bottom: 1px solid var(--line);
        }
        .top .wrap { display: flex; align-items: center; gap: 14px 20px; flex-wrap: wrap; padding-top: 12px; padding-bottom: 12px; }
        .eyebrow { font-size: 11px; letter-spacing: .16em; text-transform: uppercase; color: var(--muted); }
        h1 { margin: 0; font-size: 26px; font-weight: 700; line-height: 1.1; }
        .spacer { flex: 1 1 auto; }
        .pill {
            display: inline-flex; align-items: center; gap: 8px;
            padding: 6px 12px; border-radius: 999px;
            border: 1px solid var(--line); background: var(--surface);
            font-size: 13px; color: var(--muted); white-space: nowrap;
        }
        .pill strong { color: var(--text); font-weight: 700; letter-spacing: .04em; }
        .pill.is-live { border-color: rgba(255, 176, 32, .5); background: var(--live-bg); }
        .pill.is-live strong { color: var(--live); }
        .pill.is-warn { border-color: rgba(255, 107, 107, .5); background: var(--loss-bg); color: var(--text); }
        .dot { width: 8px; height: 8px; border-radius: 50%; background: var(--live); flex: 0 0 auto; animation: pulse 1.6s ease-in-out infinite; }
        .classic { font-size: 13px; color: var(--muted); text-decoration: none; border-bottom: 1px solid var(--line); padding-bottom: 1px; }
        .classic:hover { color: var(--text); border-color: var(--muted); }

        /* ---------- sections ---------- */
        main { padding: 18px 0 40px; }
        section { margin-top: 22px; }
        .sec-head { display: flex; align-items: center; gap: 12px 18px; flex-wrap: wrap; margin-bottom: 10px; }
        h2 { margin: 0; font-size: 13px; font-weight: 700; letter-spacing: .16em; text-transform: uppercase; color: var(--muted); }

        .stats { display: grid; grid-template-columns: repeat(auto-fit, minmax(170px, 1fr)); gap: 12px; margin-top: 0; }
        .stat { background: var(--surface); border: 1px solid var(--line-soft); border-radius: 12px; padding: 12px 14px; min-width: 0; }
        .stat .k { font-size: 11px; letter-spacing: .14em; text-transform: uppercase; color: var(--muted); }
        .stat .v { font-size: 22px; font-weight: 700; margin-top: 2px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
        .stat .s { font-size: 13px; color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }

        /* ---------- show / hide bar for the NFL live scoring cards ---------- */
        .fold-h { margin: 0; font-size: inherit; letter-spacing: normal; text-transform: none; }
        .fold {
            display: flex; align-items: center; gap: 12px; width: 100%;
            background: var(--surface); border: 1px solid var(--line-soft); border-radius: 12px;
            padding: 10px 14px; color: var(--text); font: inherit; text-align: left; cursor: pointer;
        }
        .fold:hover { background: #151c26; }
        .fold:focus-visible { outline: 2px solid var(--live); outline-offset: 2px; }
        .chev { flex: 0 0 auto; width: 8px; height: 8px; margin: 0 4px 3px 3px; border-right: 2px solid var(--muted); border-bottom: 2px solid var(--muted); transform: rotate(-45deg); transition: transform .15s; }
        .fold[aria-expanded="true"] .chev { transform: rotate(45deg); margin-bottom: 5px; }
        .fold-text { flex: 1 1 auto; min-width: 0; display: flex; flex-direction: column; gap: 1px; }
        .fold-title { font-size: 13px; font-weight: 700; letter-spacing: .14em; text-transform: uppercase; color: var(--text); }
        .fold-sum { display: inline-flex; align-items: center; gap: 7px; font-size: 13px; font-weight: 400; color: var(--muted); }
        .fold-act { flex: 0 0 auto; font-size: 13px; font-weight: 700; color: var(--text); border: 1px solid var(--line); border-radius: 8px; padding: 4px 12px; background: var(--raised); }
        th.sorted { color: var(--text); box-shadow: inset 0 -2px 0 var(--live); }

        /* ---------- game cards ---------- */
        .games { display: grid; grid-template-columns: repeat(auto-fill, minmax(255px, 1fr)); gap: 12px; margin-top: 12px; }
        .game {
            position: relative; background: var(--surface);
            border: 1px solid var(--line-soft); border-top: 2px solid var(--line);
            border-radius: 12px; padding: 10px 14px 12px;
            cursor: pointer; min-width: 0; display: flex; flex-direction: column;
            transition: border-color .15s, background .15s;
        }
        .game:hover { background: #151c26; }
        .game:focus-visible { outline: 2px solid var(--live); outline-offset: 2px; }
        .game.is-in { border-top-color: var(--live); }
        .game.is-post { border-top-color: #3b4a5e; }
        .game.hl { border-color: #5b6f8a; border-top-color: #9fb4d0; background: #172030; }

        .g-top { display: flex; align-items: center; gap: 8px; font-size: 12px; color: var(--muted); letter-spacing: .06em; text-transform: uppercase; min-height: 18px; }
        .g-top .when { font-weight: 700; color: var(--text); display: inline-flex; align-items: center; gap: 7px; }
        .game.is-in .g-top .when { color: var(--live); }
        .game.is-post .g-top .when { color: var(--muted); }
        .g-top .net { margin-left: auto; }

        .team { display: grid; grid-template-columns: 30px 1fr auto; align-items: center; gap: 10px; padding-top: 8px; }
        .logo { width: 30px; height: 30px; border-radius: 50%; background: #e7ecf2; display: grid; place-items: center; overflow: hidden; }
        .logo img { max-width: 22px; max-height: 22px; display: block; }
        .logo span { font-size: 10px; font-weight: 700; color: #1b2330; }
        .t-name { min-width: 0; display: flex; align-items: baseline; gap: 7px; }
        .t-name b { font-size: 17px; font-weight: 700; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
        .t-name small { font-size: 12px; color: var(--faint); }
        .t-score { font-size: 26px; font-weight: 700; font-family: var(--font-tight); line-height: 1; min-width: 1.4em; text-align: right; border-radius: 6px; padding: 1px 3px; }
        .team.lost b, .team.lost .t-score { color: var(--faint); font-weight: 400; }
        .t-score.flash { animation: flash 2.2s ease-out; }
        .ball { display: inline-block; width: 11px; height: 7px; border-radius: 50%; background: var(--live); transform: rotate(-28deg); flex: 0 0 auto; align-self: center; }

        .sit { margin-top: 10px; padding-top: 9px; border-top: 1px solid var(--line-soft); }
        .sit-line { display: flex; align-items: center; gap: 8px; font-size: 14px; font-weight: 700; min-height: 20px; }
        .rz { font-size: 10px; letter-spacing: .12em; color: #fff; background: #c93838; border-radius: 4px; padding: 2px 5px; }
        .field { position: relative; display: flex; height: 22px; margin-top: 7px; border-radius: 5px; overflow: hidden; border: 1px solid #224a34; }
        .ez { flex: 0 0 9%; display: grid; place-items: center; font-size: 9px; font-weight: 700; letter-spacing: .06em; color: rgba(255, 255, 255, .75); background: #1d4a33; }
        .turf { position: relative; flex: 1 1 auto; background: var(--turf) repeating-linear-gradient(90deg, transparent 0, transparent calc(10% - 1px), var(--turf-line) calc(10% - 1px), var(--turf-line) 10%); }
        .drive { position: absolute; top: 0; bottom: 0; background: linear-gradient(90deg, rgba(255, 176, 32, .38), rgba(255, 176, 32, .04)); }
        .drive.l { background: linear-gradient(270deg, rgba(255, 176, 32, .38), rgba(255, 176, 32, .04)); }
        .marker { position: absolute; top: 50%; width: 12px; height: 8px; margin: -4px 0 0 -6px; border-radius: 50%; background: var(--live); box-shadow: 0 0 0 2px rgba(10, 14, 19, .8); }
        .last { margin-top: 6px; font-size: 12px; color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }

        .split { margin-top: auto; padding-top: 10px; }
        .split-bar { display: flex; height: 5px; border-radius: 3px; overflow: hidden; background: var(--raised); gap: 2px; }
        .split-bar i { display: block; background: #4a596d; min-width: 0; }
        .split-bar i.good { background: var(--win); }
        .split-bar i.good.soft { background: rgba(53, 208, 127, .55); }
        .split-txt { display: flex; justify-content: space-between; margin-top: 4px; font-size: 12px; color: var(--muted); }
        .split-txt b { color: var(--text); font-weight: 700; }

        /* ---------- leaderboard ---------- */
        .seg { display: inline-flex; border: 1px solid var(--line); border-radius: 8px; overflow: hidden; }
        .seg button { background: transparent; color: var(--muted); border: 0; font: inherit; font-size: 13px; padding: 5px 12px; cursor: pointer; }
        .seg button[aria-pressed="true"] { background: var(--raised); color: var(--text); font-weight: 700; }
        .legend { display: flex; gap: 6px 14px; flex-wrap: wrap; font-size: 12px; color: var(--muted); margin-left: auto; align-items: center; }
        .legend span { display: inline-flex; align-items: center; gap: 6px; }
        .legend .chip { font-style: normal; font-size: 11px; min-width: 0; padding: 2px 7px; }

        .table-wrap { overflow-x: auto; border: 1px solid var(--line-soft); border-radius: 12px; background: var(--surface); }
        table { border-collapse: separate; border-spacing: 0; width: 100%; font-size: 14px; }
        th, td { padding: 6px 4px; text-align: center; border-bottom: 1px solid var(--line-soft); white-space: nowrap; }
        tbody tr:last-child td { border-bottom: 0; }
        thead th { font-size: 11px; font-weight: 700; letter-spacing: .1em; text-transform: uppercase; color: var(--muted); vertical-align: bottom; background: var(--surface); padding-top: 10px; padding-bottom: 8px; border-bottom-color: var(--line); }
        .c-rank { width: 44px; min-width: 44px; position: sticky; left: 0; z-index: 2; background: var(--surface); color: var(--muted); }
        .c-name { text-align: left; padding-left: 6px; padding-right: 14px; position: sticky; left: 44px; z-index: 2; background: var(--surface); border-right: 1px solid var(--line-soft); font-size: 15px; }
        thead .c-rank, thead .c-name { z-index: 3; }
        .c-name small { color: var(--faint); font-size: 12px; margin-left: 6px; }
        .c-pts { width: 48px; min-width: 48px; font-size: 16px; font-weight: 700; }
        .c-pts.live { color: var(--live); }
        .c-pts.dim { color: var(--muted); font-weight: 400; font-size: 14px; }
        .c-gap { border-right: 1px solid var(--line-soft); }
        tr.lead .c-rank { color: var(--live); font-weight: 700; }
        tr.part td { color: var(--faint); }
        tbody tr:hover td { background: #161e29; }

        th.g { cursor: pointer; min-width: 52px; padding-left: 3px; padding-right: 3px; letter-spacing: .03em; text-transform: none; }
        th.g .m { display: grid; grid-template-columns: auto auto; justify-content: center; gap: 0 5px; font-size: 12px; line-height: 1.25; color: var(--text); }
        th.g .m span:nth-child(even) { color: var(--muted); font-weight: 400; text-align: right; }
        th.g .st { display: block; margin-top: 3px; font-size: 10px; font-weight: 400; letter-spacing: .04em; color: var(--faint); text-transform: uppercase; }
        th.g.is-in .st { color: var(--live); font-weight: 700; }
        th.g.hl, td.hl { background: #172030 !important; }

        .chip { display: inline-block; min-width: 40px; padding: 3px 2px; border-radius: 6px; border: 1px solid transparent; font-size: 13px; font-weight: 700; letter-spacing: .02em; line-height: 1.2; }
        .chip.win { background: var(--win-bg); color: var(--win); border-color: rgba(53, 208, 127, .38); }
        .chip.loss { color: var(--loss); text-decoration: line-through; text-decoration-thickness: 1px; opacity: .85; font-weight: 400; }
        .chip.ahead { color: var(--win); border: 1px dashed rgba(53, 208, 127, .75); }
        .chip.behind { color: var(--loss); border: 1px dashed rgba(255, 107, 107, .65); }
        .chip.tied { color: var(--live); border: 1px dashed rgba(255, 176, 32, .7); }
        .chip.pending { background: var(--raised); color: var(--text); font-weight: 400; }
        .chip.none { color: var(--faint); font-weight: 400; }

        .note { margin-top: 10px; font-size: 12px; color: var(--faint); }
        .banner { background: var(--loss-bg); border: 1px solid rgba(255, 107, 107, .4); border-radius: 10px; padding: 10px 14px; margin-bottom: 14px; font-size: 14px; }
        .empty { padding: 26px 14px; color: var(--muted); text-align: center; }
        [hidden] { display: none !important; }

        @keyframes pulse { 0%, 100% { opacity: 1; transform: scale(1); } 50% { opacity: .35; transform: scale(.8); } }
        @keyframes flash { 0% { background: rgba(255, 176, 32, .55); color: #fff; } 100% { background: transparent; } }

        @media (max-width: 640px) {
            .wrap { padding: 0 12px; }
            h1 { font-size: 22px; }
            .games { grid-template-columns: 1fr; }
            .stats { grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 8px; }
            .stat { padding: 8px 10px; border-radius: 10px; }
            .stat .k { font-size: 9px; letter-spacing: .1em; }
            .stat .v { font-size: 16px; }
            .stat .s { font-size: 11px; }
            .legend { margin-left: 0; }
            .c-name { max-width: 130px; overflow: hidden; text-overflow: ellipsis; }
        }
        @media (prefers-reduced-motion: reduce) {
            .dot, .t-score.flash { animation: none; }
            .game { transition: none; }
        }
    </style>
</head>
<body>
    <header class="top">
        <div class="wrap">
            <div>
                <div class="eyebrow">Bell Football Pool &middot; <%= Year %></div>
                <h1>Week <%= WeekNumber %> Live</h1>
            </div>
            <div class="spacer"></div>
            <a class="classic" href="<%= ClassicPage %>">Classic view</a>
            <div id="status" class="pill" role="status" aria-live="polite">Loading&hellip;</div>
        </div>
    </header>

    <main class="wrap">
        <div id="banner" class="banner" hidden></div>

        <section id="stats" class="stats" aria-label="Week summary"></section>

        <section>
            <h2 class="fold-h">
                <button type="button" id="games-toggle" class="fold" aria-expanded="false" aria-controls="games">
                    <span class="chev" aria-hidden="true"></span>
                    <span class="fold-text">
                        <span class="fold-title">NFL live scoring updates</span>
                        <span id="games-sum" class="fold-sum num">Scores, clock and possession for every game</span>
                    </span>
                    <span id="games-act" class="fold-act">Show</span>
                </button>
            </h2>
            <div id="games" class="games" hidden></div>
        </section>

        <section aria-labelledby="board-h">
            <div class="sec-head">
                <h2 id="board-h">Leaderboard</h2>
                <div class="seg" role="group" aria-label="Sort players">
                    <button type="button" data-sort="name" aria-pressed="true">A&ndash;Z</button>
                    <button type="button" data-sort="season" aria-pressed="false">Season</button>
                    <button type="button" data-sort="week" aria-pressed="false">Weekly</button>
                </div>
                <div class="legend" aria-label="Legend">
                    <span><i class="chip win">WON</i></span>
                    <span><i class="chip loss">LOST</i></span>
                    <span><i class="chip ahead">AHEAD</i></span>
                    <span><i class="chip behind">BEHIND</i></span>
                    <span><i class="chip tied">TIED</i></span>
                    <span><i class="chip pending">TO PLAY</i></span>
                </div>
            </div>
            <div class="table-wrap"><table id="board"><tbody><tr><td class="empty">Loading picks&hellip;</td></tr></tbody></table></div>
            <p class="note">Scores come from ESPN and refresh on their own while games are live. Picks and game order come from the pool. Tap a game heading to highlight its column.</p>
        </section>
    </main>

    <script>
    (function () {
        'use strict';

        var DATA_URL = location.pathname + '?data=1';
        var LOGO_DIR = '../img/';

        var SORTS = ['name', 'season', 'week'];

        var state = {
            data: null,
            sort: 'name',        // A-Z unless the visitor has chosen otherwise
            showGames: false,    // the NFL live scoring cards start hidden
            hl: null,
            prev: {},
            lastOk: 0,
            failures: 0,
            lastError: '',
            timer: null
        };

        try {
            var savedSort = localStorage.getItem('poolLiveSort');
            if (SORTS.indexOf(savedSort) >= 0) { state.sort = savedSort; }
            state.showGames = localStorage.getItem('poolLiveShowGames') === '1';
        } catch (e) { }

        var $ = function (id) { return document.getElementById(id); };

        function esc(value) {
            return String(value == null ? '' : value).replace(/[&<>"']/g, function (c) {
                return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
            });
        }

        function kickoffText(iso, withDay) {
            if (!iso) { return ''; }
            var d = new Date(iso);
            if (isNaN(d)) { return ''; }
            var time = d.toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' });
            return withDay ? d.toLocaleDateString([], { weekday: 'short' }) + ' ' + time : time;
        }

        // How one pick is doing: win, loss, ahead, behind, tied, pending, none.
        function outcome(code, game) {
            if (code !== 'a' && code !== 'h') { return 'none'; }
            if (game.state === 'pre') { return 'pending'; }
            var mine = code === 'a' ? game.away.score : game.home.score;
            var theirs = code === 'a' ? game.home.score : game.away.score;
            if (mine == null || theirs == null) { return 'pending'; }
            if (game.state === 'post') { return mine > theirs ? 'win' : 'loss'; }
            return mine > theirs ? 'ahead' : mine < theirs ? 'behind' : 'tied';
        }

        function compute(data) {
            var games = data.games;
            var players = data.players.map(function (p) {
                var res = p.picks.map(function (code, i) { return outcome(code, games[i]); });
                var wins = 0, ahead = 0, open = 0, picked = 0;
                res.forEach(function (r) {
                    if (r !== 'none') { picked++; }
                    if (r === 'win') { wins++; }
                    if (r === 'ahead') { ahead++; }
                    if (r === 'ahead' || r === 'behind' || r === 'tied' || r === 'pending') { open++; }
                });
                return {
                    name: (p.first + ' ' + p.last).replace(/\s+/g, ' ').trim(),
                    first: p.first, last: p.last, picks: p.picks, res: res,
                    wins: wins, now: wins + ahead, max: wins + open, picked: picked,
                    partial: picked * 2 < games.length,
                    // Season score plus this week as it stands. Any score already saved for this week is
                    // taken out first so it is never counted twice.
                    season: p.seasonTotal == null ? null : p.seasonTotal - (p.seasonWeek || 0) + wins + ahead
                };
            });

            function byName(a, b) {
                return a.last.toUpperCase().localeCompare(b.last.toUpperCase()) || a.first.toUpperCase().localeCompare(b.first.toUpperCase());
            }

            var alphabetical = players.slice().sort(byName);

            // Weekly standings: by the count as it stands, ties share a place.
            var ranked = players.slice().sort(function (a, b) {
                return b.now - a.now || b.wins - a.wins || b.max - a.max || byName(a, b);
            });
            ranked.forEach(function (p, i) {
                var prev = ranked[i - 1];
                p.rank = prev && prev.now === p.now ? prev.rank : i + 1;
            });
            ranked.forEach(function (p) {
                p.tie = ranked.filter(function (q) { return q.rank === p.rank; }).length > 1;
            });

            // Season standings: season score plus the current weekly score. Players with no season row go last.
            var seasonRanked = players.slice().sort(function (a, b) {
                if ((a.season == null) !== (b.season == null)) { return a.season == null ? 1 : -1; }
                return (b.season || 0) - (a.season || 0) || b.now - a.now || byName(a, b);
            });
            seasonRanked.forEach(function (p, i) {
                var prev = seasonRanked[i - 1];
                p.srank = p.season == null ? null : prev && prev.season === p.season ? prev.srank : i + 1;
            });
            seasonRanked.forEach(function (p) {
                p.stie = p.srank != null && seasonRanked.filter(function (q) { return q.srank === p.srank; }).length > 1;
            });

            return { players: players, alphabetical: alphabetical, ranked: ranked, seasonRanked: seasonRanked };
        }

        // ------------------------------------------------------------ render

        function renderStatus() {
            var el = $('status');
            var d = state.data;
            if (!d) {
                el.className = 'pill' + (state.failures ? ' is-warn' : '');
                el.textContent = state.failures ? 'Cannot reach the pool. Retrying\u2026' : 'Loading\u2026';
                return;
            }

            var age = Math.max(0, Math.round((Date.now() - state.lastOk) / 1000));
            var ago = age < 5 ? 'just now' : age < 90 ? age + 's ago' : Math.round(age / 60) + ' min ago';
            var stale = state.failures > 0 || !d.espn.ok;

            if (stale) {
                el.className = 'pill is-warn';
                el.innerHTML = '<strong>SCORES DELAYED</strong> last update ' + esc(ago);
            } else if (d.anyLive) {
                el.className = 'pill is-live';
                el.innerHTML = '<span class="dot"></span><strong>LIVE</strong> updated ' + esc(ago);
            } else if (d.allFinal) {
                el.className = 'pill';
                el.innerHTML = '<strong>FINAL</strong> all games complete';
            } else {
                var next = d.games.filter(function (g) { return g.state === 'pre' && g.kickoff; })[0];
                el.className = 'pill';
                el.innerHTML = next ? 'Next kickoff <strong>' + esc(kickoffText(next.kickoff, true)) + '</strong>' : 'Updated ' + esc(ago);
            }
        }

        // Logos: the pool's own file first, then the address ESPN supplies, then the abbreviation as text.
        // A file found missing once is remembered so it is not requested again on every refresh.
        var noLocalLogo = {};

        function localLogo(team) {
            return LOGO_DIR + team.logo + '.png';
        }

        function logoHtml(team) {
            var useLocal = team.logo && !noLocalLogo[team.logo];
            var src = useLocal ? localLogo(team) : (team.logoAlt || '');
            if (!src) { return '<span class="logo"><span>' + esc(team.abbr) + '</span></span>'; }
            return '<span class="logo"><img src="' + esc(src) + '" alt="" data-key="' + esc(team.logo) + '" data-abbr="' + esc(team.abbr) + '"' +
                (useLocal && team.logoAlt ? ' data-alt="' + esc(team.logoAlt) + '"' : '') + '></span>';
        }

        document.addEventListener('error', function (event) {
            var img = event.target;
            if (!img || img.tagName !== 'IMG' || !img.parentNode || img.parentNode.className !== 'logo') { return; }
            var alt = img.getAttribute('data-alt');
            if (alt) {
                noLocalLogo[img.getAttribute('data-key')] = true;
                img.removeAttribute('data-alt');
                img.src = alt;
            } else {
                var text = document.createElement('span');
                text.textContent = img.getAttribute('data-abbr') || '';
                img.parentNode.replaceChild(text, img);
            }
        }, true);

        function teamRow(team, other, game, key) {
            var lost = game.state === 'post' && team.score != null && other.score != null && team.score < other.score;
            var score = team.score == null ? '' : team.score;
            var prev = state.prev[key];
            var flash = prev !== undefined && prev !== score && game.state === 'in' ? ' flash' : '';
            state.prev[key] = score;

            return '<div class="team' + (lost ? ' lost' : '') + '">' +
                logoHtml(team) +
                '<span class="t-name"><b>' + esc(team.nick || team.abbr) + '</b>' +
                (team.ball ? '<span class="ball" title="Has the ball"></span>' : '') +
                (team.record ? '<small class="num">' + esc(team.record) + '</small>' : '') + '</span>' +
                '<span class="t-score num' + flash + '">' + esc(score) + '</span>' +
                '</div>';
        }

        function situation(game) {
            var s = game.sit;
            if (game.state !== 'in' || !s) { return ''; }

            var line = s.text || s.down || '';
            var html = '<div class="sit">';
            html += '<div class="sit-line">' + (line ? '<span>' + esc(line) + '</span>' : '<span>' + esc(game.status) + '</span>') +
                (s.redZone ? '<span class="rz">RED ZONE</span>' : '') + '</div>';

            if (s.x != null) {
                var x = Math.max(0, Math.min(100, s.x));
                var drive = '';
                if (s.dir === 'r') { drive = '<span class="drive" style="left:' + x + '%;right:0"></span>'; }
                if (s.dir === 'l') { drive = '<span class="drive l" style="left:0;right:' + (100 - x) + '%"></span>'; }
                html += '<div class="field" role="img" aria-label="Ball at ' + esc(s.spot || '') + '">' +
                    '<span class="ez">' + esc(game.away.abbr) + '</span>' +
                    '<span class="turf">' + drive + '<span class="marker" style="left:' + x + '%"></span></span>' +
                    '<span class="ez">' + esc(game.home.abbr) + '</span></div>';
            }

            if (s.lastPlay) { html += '<div class="last" title="' + esc(s.lastPlay) + '">' + esc(s.lastPlay) + '</div>'; }
            return html + '</div>';
        }

        function split(game, index, players) {
            var a = 0, h = 0;
            players.forEach(function (p) { if (p.picks[index] === 'a') { a++; } if (p.picks[index] === 'h') { h++; } });
            if (a + h === 0) { return ''; }

            var lead = '';
            if (game.state !== 'pre' && game.away.score != null && game.home.score != null && game.away.score !== game.home.score) {
                lead = game.away.score > game.home.score ? 'a' : 'h';
            }
            var soft = game.state === 'in' ? ' soft' : '';

            return '<div class="split">' +
                '<div class="split-bar" aria-hidden="true">' +
                '<i style="flex:' + a + '"' + (lead === 'a' ? ' class="good' + soft + '"' : '') + '></i>' +
                '<i style="flex:' + h + '"' + (lead === 'h' ? ' class="good' + soft + '"' : '') + '></i></div>' +
                '<div class="split-txt num"><span><b>' + a + '</b> picked ' + esc(game.away.abbr) + '</span><span>' + esc(game.home.abbr) + ' <b>' + h + '</b></span></div>' +
                '</div>';
        }

        function syncGamesToggle() {
            var toggle = $('games-toggle');
            toggle.setAttribute('aria-expanded', String(state.showGames));
            $('games-act').textContent = state.showGames ? 'Hide' : 'Show';
            $('games').hidden = !state.showGames;
        }

        function renderGames(data, calc) {
            var fin = data.games.filter(function (g) { return g.state === 'post'; }).length;
            var live = data.games.filter(function (g) { return g.state === 'in'; }).length;
            var left = data.games.length - fin - live;
            var parts = [];
            if (live) { parts.push(live + ' live'); }
            if (fin) { parts.push(fin + ' final'); }
            if (left) { parts.push(left + ' to play'); }
            $('games-sum').innerHTML = (live ? '<span class="dot"></span>' : '') + esc(parts.join(' \u00b7 ') || 'No games found');

            syncGamesToggle();
            if (!state.showGames) { return; }   // nothing to draw while the section is folded away

            var html = data.games.map(function (g, i) {
                var when;
                if (g.state === 'in') { when = '<span class="dot"></span>' + esc(g.status); }
                else if (g.state === 'post') { when = esc(g.status || 'Final'); }
                else { when = esc(kickoffText(g.kickoff, true) || g.day || 'Scheduled'); }

                return '<div role="button" tabindex="0" class="game is-' + g.state + (state.hl === i ? ' hl' : '') + '" data-g="' + i + '" aria-pressed="' + (state.hl === i) + '" aria-label="' + esc(g.away.nick + ' at ' + g.home.nick + ', highlight column') + '">' +
                    '<div class="g-top"><span class="when num">' + when + '</span>' +
                    (g.network && g.state !== 'post' ? '<span class="net">' + esc(g.network) + '</span>' : '') + '</div>' +
                    teamRow(g.away, g.home, g, g.col + 'a') +
                    teamRow(g.home, g.away, g, g.col + 'h') +
                    situation(g) +
                    split(g, i, calc.players) +
                    '</div>';
            }).join('');

            $('games').innerHTML = html || '<div class="empty">No games found for this week.</div>';
        }

        function renderStats(data, calc) {
            var games = data.games;
            var fin = games.filter(function (g) { return g.state === 'post'; }).length;
            var live = games.filter(function (g) { return g.state === 'in'; }).length;
            var left = games.length - fin - live;

            var full = calc.ranked.filter(function (p) { return !p.partial; });
            var leaders = full.filter(function (p) { return p.rank === (full[0] ? full[0].rank : 0); });
            var avg = full.length ? full.reduce(function (t, p) { return t + p.now; }, 0) / full.length : 0;
            var started = fin + live > 0;

            var tiles = [
                { k: 'Games', v: fin + ' of ' + games.length + ' final', s: live ? live + ' live now, ' + left + ' to play' : left ? left + ' to play' : 'Week complete' },
                { k: started ? (leaders.length > 1 ? 'Tied for the lead' : 'Leader') : 'Entries',
                  v: started && leaders.length ? (leaders.length > 2 ? leaders.length + ' players' : leaders.map(function (p) { return p.name; }).join(' & ')) : full.length + ' players',
                  s: started && leaders.length ? leaders[0].now + ' correct' + (data.anyLive ? ' as it stands' : '') : 'Picks are in' },
                { k: 'Pool average', v: started ? avg.toFixed(1) : '\u2013', s: started ? 'correct picks per player' : 'No games played yet' }
            ];

            $('stats').innerHTML = tiles.map(function (t) {
                return '<div class="stat"><div class="k">' + esc(t.k) + '</div><div class="v num" title="' + esc(t.v) + '">' + esc(t.v) + '</div><div class="s">' + esc(t.s) + '</div></div>';
            }).join('');
        }

        var LABEL = { win: 'won', loss: 'lost', ahead: 'winning now', behind: 'losing now', tied: 'tied', pending: 'not started', none: 'no pick' };

        function renderBoard(data, calc) {
            var games = data.games;
            var showNow = data.anyLive;
            var showSeason = calc.players.some(function (p) { return p.season != null; });

            // The Season choice only makes sense once the season table has players in it.
            var seasonButton = document.querySelector('[data-sort="season"]');
            if (seasonButton) { seasonButton.hidden = !showSeason; }
            var sort = state.sort === 'season' && !showSeason ? 'name' : state.sort;
            syncSortButtons(sort);

            var rows = sort === 'season' ? calc.seasonRanked : sort === 'week' ? calc.ranked : calc.alphabetical;
            var bySeason = sort === 'season';
            var weekKey = showNow ? 'now' : 'wk';
            var mark = function (key) { return (bySeason ? key === 'ssn' : sort === 'week' && key === weekKey) ? ' sorted' : ''; };

            var head = '<thead><tr><th class="c-rank" title="' + (bySeason ? 'Place in the season standings' : 'Place this week') + '">#</th>' +
                '<th class="c-name' + (sort === 'name' ? ' sorted' : '') + '">Player</th>' +
                '<th class="c-pts' + mark('wk') + '" title="Correct picks in finished games">Wk</th>' +
                (showNow ? '<th class="c-pts' + mark('now') + '" title="Correct picks if every game ended right now">Now</th>' : '') +
                '<th class="c-pts" title="Best possible total this week">Max</th>' +
                (showSeason ? '<th class="c-pts c-gap' + mark('ssn') + '" title="Season score plus this week as it stands">Ssn</th>' : '');

            games.forEach(function (g, i) {
                var st = g.state === 'in' ? g.status : g.state === 'post' ? 'Final' : kickoffText(g.kickoff, false) || g.day;
                var a = g.away.score == null ? '' : g.away.score, h = g.home.score == null ? '' : g.home.score;
                head += '<th class="g is-' + g.state + (state.hl === i ? ' hl' : '') + '" data-g="' + i + '" title="' + esc(g.away.nick + ' at ' + g.home.nick) + '">' +
                    '<span class="m num"><span>' + esc(g.away.abbr) + '</span><span>' + esc(a) + '</span><span>' + esc(g.home.abbr) + '</span><span>' + esc(h) + '</span></span>' +
                    '<span class="st num">' + esc(st) + '</span></th>';
            });
            head += '</tr></thead>';

            var body = '<tbody>' + rows.map(function (p) {
                var place = bySeason ? p.srank : p.rank;
                var tied = bySeason ? p.stie : p.tie;
                var lead = bySeason ? p.srank === 1 : p.rank === 1 && p.now > 0 && !p.partial;
                var cells = '<td class="c-rank num">' + (place == null ? '\u2013' : (tied ? 'T' : '') + place) + '</td>' +
                    '<td class="c-name">' + esc(p.name) + (p.partial ? '<small>' + p.picked + ' pick' + (p.picked === 1 ? '' : 's') + '</small>' : '') + '</td>' +
                    '<td class="c-pts num">' + p.wins + '</td>' +
                    (showNow ? '<td class="c-pts live num">' + p.now + '</td>' : '') +
                    '<td class="c-pts dim num">' + p.max + '</td>' +
                    (showSeason ? '<td class="c-pts dim c-gap num">' + (p.season == null ? '\u2013' : p.season) + '</td>' : '');

                p.res.forEach(function (r, i) {
                    var g = games[i];
                    var code = p.picks[i];
                    var text = code === 'a' ? g.away.abbr : code === 'h' ? g.home.abbr : code === '?' ? '?' : '\u2013';
                    cells += '<td' + (state.hl === i ? ' class="hl"' : '') + '><span class="chip ' + r + '" title="' + esc(text + ': ' + LABEL[r]) + '">' + esc(text) + '</span></td>';
                });

                return '<tr class="' + (lead ? 'lead' : '') + (p.partial ? ' part' : '') + '">' + cells + '</tr>';
            }).join('') + '</tbody>';

            $('board').innerHTML = rows.length ? head + body : '<tbody><tr><td class="empty">No picks have been entered for this week yet.</td></tr></tbody>';
        }

        function render() {
            var d = state.data;
            if (!d) { return; }
            var calc = compute(d);
            renderStats(d, calc);
            renderGames(d, calc);
            renderBoard(d, calc);
            renderStatus();

            var banner = $('banner');
            if (!d.espn.ok) {
                banner.hidden = false;
                banner.textContent = d.espn.ageSeconds == null
                    ? 'Live scores are not available right now. Picks are shown without scores; the classic view still works.'
                    : 'Live scores are temporarily unavailable. Showing the last scores received.';
            } else {
                banner.hidden = true;
            }
        }

        // ------------------------------------------------------------ polling

        function schedule(seconds) {
            clearTimeout(state.timer);
            if (seconds > 0 && !document.hidden) {
                state.timer = setTimeout(load, seconds * 1000);
            }
        }

        function load() {
            clearTimeout(state.timer);
            fetch(DATA_URL + '&_=' + Date.now(), { cache: 'no-store', headers: { 'Accept': 'application/json' } })
                .then(function (response) {
                    return response.json().then(function (body) {
                        if (!response.ok || body.error) { throw new Error(body.error || 'HTTP ' + response.status); }
                        return body;
                    });
                })
                .then(function (body) {
                    state.data = body;
                    state.lastOk = Date.now();
                    state.failures = 0;
                    render();
                    schedule(body.pollSeconds);
                })
                .catch(function (error) {
                    state.failures++;
                    state.lastError = String(error && error.message || error);
                    renderStatus();
                    if (!state.data) {
                        $('banner').hidden = false;
                        $('banner').textContent = 'The live page could not load its data. It will keep trying. The classic view may still work.';
                    }
                    schedule(Math.min(120, 15 * state.failures));
                });
        }

        document.addEventListener('click', function (event) {
            var sortButton = event.target.closest('[data-sort]');
            if (sortButton) {
                state.sort = sortButton.getAttribute('data-sort');
                try { localStorage.setItem('poolLiveSort', state.sort); } catch (e) { }
                syncSortButtons(state.sort);
                render();
                return;
            }

            if (event.target.closest('#games-toggle')) {
                state.showGames = !state.showGames;
                try { localStorage.setItem('poolLiveShowGames', state.showGames ? '1' : '0'); } catch (e) { }
                syncGamesToggle();
                render();
                return;
            }

            var gameEl = event.target.closest('[data-g]');
            if (gameEl) {
                var index = parseInt(gameEl.getAttribute('data-g'), 10);
                state.hl = state.hl === index ? null : index;
                render();
            }
        });

        document.addEventListener('keydown', function (event) {
            if ((event.key === 'Enter' || event.key === ' ') && event.target.classList && event.target.classList.contains('game')) {
                event.preventDefault();
                event.target.click();
            }
        });

        function syncSortButtons(active) {
            Array.prototype.forEach.call(document.querySelectorAll('[data-sort]'), function (button) {
                button.setAttribute('aria-pressed', String(button.getAttribute('data-sort') === active));
            });
        }

        // Stop asking while the tab is in the background; catch up when it comes back.
        document.addEventListener('visibilitychange', function () {
            if (document.hidden) { clearTimeout(state.timer); } else { load(); }
        });

        syncSortButtons(state.sort);
        syncGamesToggle();
        setInterval(renderStatus, 1000);
        load();
    })();
    </script>
</body>
</html>
