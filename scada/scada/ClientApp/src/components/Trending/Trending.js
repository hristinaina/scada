import React, { Component } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Trending.css';
import '../../fonts.css';
import { HubConnectionBuilder } from "@microsoft/signalr";


export class Trending extends Component {
  static displayName = Trending.name;

  constructor(props) {
      super(props);

      this.state = {
         tags: []
      };

      this.connection = new HubConnectionBuilder()
          .withUrl("http://localhost:5083/Hub/tag") 
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

      this.connection.on("ReceiveMessage", (tag) => {
          const currentTags = [...this.state.tags];
          const existingTagIndex = currentTags.findIndex(t => t.tagName === tag.tagName);

          if (existingTagIndex === -1) {
              currentTags.push(tag);
          } else {
              currentTags[existingTagIndex].value = tag.value;
          }

          this.setState({ tags: currentTags });   
      });

      this.interval = setInterval(this.renderForecastsTable, 1000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
        this.connection.stop();
    }


    static renderForecastsTable(tags) {
        if (!tags || tags.length === 0) {
            return <p>No data available.</p>;
        }
        return (
          <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
              <tr>
                <th>Tag name</th>
                <th>Type</th>
                <th>Description</th>
                <th>Scan Time (ms)</th>
                <th>Range</th>
                <th>Value</th>
                <th>ALARM</th>
              </tr>
            </thead>
            <tbody>
              {tags.map(tag =>
                <tr key={tag.tagName}>
                  <td>{tag.tagName}</td>
                  <td>{tag.type}</td>
                  <td>{tag.description}</td>
                  <td>{tag.scanTime}</td>
                  <td>{tag.range}</td>
                  <td>{tag.value}</td>
                  <td></td>
                </tr>
              )}
            </tbody>
          </table>
        );
    }

  render() {
    let contents = Trending.renderForecastsTable(this.state.tags);

    return (
        <div>
        <NavMenu showNavbar={true} />
            <h1 id="tableLabel">Trending</h1>
        {contents}
      </div>
    );
  }
}
