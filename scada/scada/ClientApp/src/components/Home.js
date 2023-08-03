import React, { Component } from 'react';
import './Home.css';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
        <div id="container">
            <p class="label">Email: </p>
            <input class="input" type="email" name="email"></input>
            <br></br>
            <p class="label">Password: </p>
            <input class="input" type="password" name="password"></input>
            <button type="submit" id="login">Log in</button>
      </div>
    );
  }
}
