﻿import React, { useState } from 'react';
import axios from 'axios';


const AITag = ({ onClose }) => {
    const [selectedDriver, setSelectedDriver] = useState('RTU'); // Default value for the driver dropdown

    const create = async () => {
        const type = "AITag";
        const name = document.getElementsByName("name")[0].value;
        const description = document.getElementsByName("description")[0].value;
        const address = document.getElementsByName("address")[0].value;
        const scanTime = document.getElementsByName("scan-time")[0].value;
        const unit = document.getElementsByName("unit")[0].value;
        const lowLimit = document.getElementsByName("low-limit")[0].value;
        const highLimit = document.getElementsByName("high-limit")[0].value;
        const driver = selectedDriver;

        const tag = {
            Type: type,
            Data: {
                Name: name,
                Description: description,
                Address: address,
                ScanTime: scanTime,
                Unit: unit,
                LowLimit: lowLimit,
                HighLimit: highLimit,
                Driver: driver
            }
        }

        console.log("TAGGGG");
        console.log(tag);

        try {
            const response = await axios.post('http://localhost:5083/api/tag', tag);

            console.log("Create successful!!!", response.data);
        }
        catch (error) {
            console.log("Create failed: ", error);
        }

        //onClose();
    };

    return (
        <div className="dialog">
            <p id="title">New Analog Input Tag</p>

            <p
                className="label">Name</p>
            <input
                className="input"
                type="text"
                name="name"
                maxLength="50"
                placeholder="Type name of the tag here..."
            ></input>

            <p className="label">Description</p>
            <textarea name="description" className="input" placeholder="Type description of the tag here..."
            />

            <p className="label">Address</p>
            <input
                className="input"
                type="text"
                name="address"
                maxLength="20"
                placeholder="Type address here..."
            ></input>

            <p
                className="label">Scan time</p>
            <input
                className="input"
                type="number"
                name="scan-time"
                maxLength="20"
                placeholder="Type scan time here..."
            ></input>

            <p className="label">Driver</p>
            <select
                className="input"
                value={selectedDriver}
                onChange={(e) => setSelectedDriver(e.target.value)}>
                <option value="RTU">Real-time</option>
                <option value="SIM">Simulation</option>
            </select>

            <p
                className="label">Unit</p>
            <input
                className="input"
                type="text"
                name="unit"
                maxLength="10"
                placeholder="Type unit here..."
            ></input>

            <p
                className="label">Low limit</p>
            <input
                className="input"
                type="number"
                name="low-limit"
                maxLength="10"
                placeholder="Type low limit here..."
            ></input>

            <p
                className="label">High limit</p>
            <input
                className="input"
                type="number"
                name="high-limit"
                maxLength="10"
                placeholder="Type high limit here..."
            ></input>


            <div id="buttons">
                <button className="btn" id="save" onClick={create}>SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default AITag;