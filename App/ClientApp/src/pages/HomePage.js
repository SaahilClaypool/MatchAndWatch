import React from 'react';
import { SessionIndexLoader } from '../components/SessionIndex';
import { Link } from 'react-router-dom'
import { Routes } from '../Routes';
import './HomePage.css'


export function HomePage() {
  return (
    <div className="homepage">
      <div className="Start">
        <StartSession />
      </div>
      <hr />
      <div className="Sessions">
        <SessionIndexLoader />
      </div>
    </div>
  )
}

function StartSession() {
  return (
    <div className="StartSession">
      <Link to={Routes.Session.New}>Start</Link>
    </div>
  )
}

