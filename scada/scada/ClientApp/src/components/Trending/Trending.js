import React, { Component } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Trending.css';
import '../../fonts.css';
import { HubConnectionBuilder } from "@microsoft/signalr";
import TrendingService from '../../services/TrendingService';


export class Trending extends Component {
  static displayName = Trending.name;

  constructor(props) {
      super(props);

      this.state = {
         tags: []
      };

      this.tagConnection = TrendingService.createConnection("http://localhost:5083/Hub/tag");
      this.alarmConnection = TrendingService.createConnection("http://localhost:5083/Hub/alarm");

      //this.connectionAlarm = new HubConnectionBuilder()
      //    .withUrl("http://localhost:5083/Hub/alarm")
      //    .build();
  }

  componentDidMount() {

      this.tagConnection?.on("ReceiveMessage", (tag) => {
          const currentTags = [...this.state.tags];
          const existingTagIndex = currentTags.findIndex(t => t.tagName === tag.tagName);

          if (existingTagIndex === -1) {
              currentTags.push(tag);
          } else {
              currentTags[existingTagIndex].value = tag.value;
          }
          this.setState({ tags: currentTags });   
      });

      this.alarmConnection?.on("nekaPoruka", (alarm) => {
          console.log(alarm)
      });

      this.interval = setInterval(this.renderTagsTable, 1000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
        this.connection.stop();
    }


    static renderTagsTable(tags) {
        if (!tags || tags.length === 0) {
            return <p>No data available</p>;
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
    let contents = Trending.renderTagsTable(this.state.tags);

    return (
        <div>
        <NavMenu showNavbar={true} />
            <h1 id="tableLabel">Trending</h1>
        {contents}
      </div>
    );
  }
}
