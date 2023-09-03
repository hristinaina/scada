import '../AOTag/AOTag.css';
import axios from 'axios';
import { useState } from 'react';


const DOTag = ({ onClose }) => {
    const [selectedValue, setSelectedValue] = useState('0'); // default value for the driver dropdown

    const isValid = (tag) => {
        if (
            tag.Data.TagName.trim() === "" ||
            tag.Data.Description.trim() === "" ||
            tag.Data.Address.trim() === ""
        ) {
            return false;
        }
        return true;
    }

    const create = async () => {
        const type = "DOTag";
        const name = document.getElementsByName("name")[0].value;
        const description = document.getElementsByName("description")[0].value;
        const address = document.getElementsByName("address")[0].value;
        const value = selectedValue;

        const tag = {
            Type: type,
            Data: {
                TagName: name,
                Description: description,
                Address: address,
                Value: value
            }
        }

        try {
            if (!isValid(tag)) {
                console.log("Inputted data is not valid!");
                return;
            }
            const response = await axios.post('http://localhost:5083/api/tag', tag);

            console.log("Tag created successfully!", response.data);
        }
        catch (error) {
            console.log("Create failed: ", error);
        }

        onClose();
    };

    return (
        <div className="dialog">
            <p id="title">New Digital Output Tag</p>

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
            <textarea
                className="input"
                name="description"
                placeholder="Type description of the tag here..."
            />

            <p className="label">Address</p>
            <input
                className="input"
                type="text"
                name="address"
                maxLength="20"
                placeholder="Type address here..."
            ></input>

            <p className="label">Value</p>
            <select
                className="input"
                value={selectedValue}
                onChange={(e) => setSelectedValue(e.target.value)}>
                <option value="0">0</option>
                <option value="1">1</option>
            </select>


            <div id="buttons">
                <button className="btn" id="save" onClick={create}>SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default DOTag;