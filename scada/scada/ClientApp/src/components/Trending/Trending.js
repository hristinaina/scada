import React, { Component } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Trending.css';
import '../../fonts.css';
import { HubConnectionBuilder } from "@microsoft/signalr";


export class Trending extends Component {
  static displayName = Trending.name;

  constructor(props) {
    super(props);
      this.state = { forecasts: [], loading: true };
      this.connection = new HubConnectionBuilder()
          .withUrl("http://localhost:5083/Hub/tag") // Postavite istu putanju kao u vašem serveru
          .build();
  }

  componentDidMount() {

      this.connection
          .start()
          .then(() => {
              console.log("Connected to WebSocket server");
          })
          .catch((error) => {
              console.error(error);
          });
      //this.populateWeatherData();

      this.connection.on("ReceiveMessage", (message) => {
          console.log("Received message:", message);
          
      });
    }

    componentWillUnmount() {
        this.connection.stop();
    }


  static renderForecastsTable(forecasts) {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>Type</th>
            <th>Description</th>
            <th>Scan Time</th>
            <th>Scan</th>
            <th>Low Limit</th>
            <th>High Limit</th>
            <th>Value</th>
            <th>ALARM</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Trending.renderForecastsTable(this.state.forecasts);

    return (
        <div>
        <NavMenu showNavbar={true} />
            <h1 id="tableLabel">Trending</h1>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
