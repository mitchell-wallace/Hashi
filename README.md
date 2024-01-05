# Hashi

Hashi is an API which pulls data from Jira and Wrike's APIs

## Project Background
Hashi was developed as part of a Work Integrated Learning program with Mudbath. This project forms a part of their custom integration between Jira and Wrike.

## Project Functionality 
Hashi provides a CRUD API and search endpoints to manage mappings between users and projects in Jira and Wrike, stored in a database. It caches external API responses for up to a minute to avoid rate limits, and extracts only required data from responses in its own API. Basic unit tests were also developed.
