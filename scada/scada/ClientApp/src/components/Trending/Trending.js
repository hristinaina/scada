import React, { Component } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Trending.css';
import '../../fonts.css';
import TrendingService from '../../services/TrendingService';

export class Trending extends Component {
  static displayName = Trending.name;

  constructor(props) {
      super(props);

      this.state = {
         tags: []
      };

      this.tagConnection = TrendingService.createConnection("http://localhost:5083/Hub/tag");
  }

  componentDidMount() {

      this.tagConnection?.on("ReceiveMessage", (tag) => {
          const currentTags = [...this.state.tags];
          const existingTagIndex = currentTags.findIndex(t => t.tagName === tag.tagName);

          if (existingTagIndex === -1) {
              currentTags.push(tag);
          } else {
              currentTags[existingTagIndex].value = tag.value;
              currentTags[existingTagIndex].alarm = tag.alarm;
          }
          this.setState({ tags: currentTags });   
      });

      this.interval = setInterval(this.renderTagsTable, 1000);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
        this.tagConnection.stop();
    }

    static getAlarmPriorityClass(priority) {
        switch (priority) {
           case 1:
                return "low-priority-row";
           case 2:
                return "medium-priority-row";
           case 3:
                return "high-priority-row";
           default:
                return "";
        }
    }


    static renderTagsTable(tags) {
        if (!tags || tags.length === 0) {
            return <p>No data available</p>;
        }

        tags.sort((a, b) => b.alarm.priority - a.alarm.priority);

        return (
          <table className="table" aria-labelledby="tableLabel">
            <thead>
              <tr>
                <th>Tag name</th>
                <th>Type</th>
                <th>Address</th>
                <th>Driver</th>
                <th>Description</th>
                <th>Scan Time (ms)</th>
                <th>Range</th>
                <th>Value</th>
                <th>ALARM</th>
              </tr>
            </thead>
            <tbody>
                {tags.map(tag =>
                <tr key={tag.tagName} className={Trending.getAlarmPriorityClass(tag.alarm.priority)}>
                    <td>{tag.tagName}</td>
                    <td>{tag.type}</td>
                    <td>{tag.address}</td>
                    <td>{tag.driver}</td>
                    <td>{tag.description}</td>
                    <td>{tag.scanTime}</td>
                    <td>{tag.range}</td>
                    <td>{tag.value}</td>
                    <td>{tag.alarm.description}</td>
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
