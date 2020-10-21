# Match and Watch

A dating-app inspired movie / show finder.


## Use case

1. Create a session

    This is your search criteria for movies

2. Invite friends

3. Rate some movies

    For each title (movie or show) you will be shown a poster, summary, ratings etc.
    You will then upvote or downvote.
    These will be given in a semi-random order.

    Once you have done at least X movies, you will finish your ratings.
    
    - If the other users are done:

        You will be able to see what movies you both like (you should get a similar subset as the first person).

        If you don't like any, you can continue swiping.
    
    - If the other users are *not* done:

        You will get a notification when they are

    TODO: super like - end search early on match.

4. View your matches

## Doing

- [x] Ingest basic movie data
- [x] Set up data structure
- [x] Add fluent validation
- [x] Add test project
- [ ] Add mediatR
- [ ] Add swagger auto docs

## Possible features

- recommender system
- super like
- arbitrary data sources
- People in your area
- Chat

## Uses

[TMDB api](https://developers.themoviedb.org/3)