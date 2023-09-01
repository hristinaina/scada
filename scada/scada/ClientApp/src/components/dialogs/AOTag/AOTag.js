import axios from 'axios';


const AOTag = ({ onClose }) => {
    const isValid = (tag) => {
        const lowLimit = parseFloat(tag.Data.LowLimit);
        const highLimit = parseFloat(tag.Data.HighLimit);

        if (lowLimit > highLimit) {
            return false;
        }
        if (
            tag.Data.Name.trim() === "" ||
            tag.Data.Description.trim() === "" ||
            tag.Data.Address.trim() === "" ||
            tag.Data.Units.trim() === "" ||
            tag.Data.HighLimit.trim() === "" ||
            tag.Data.LowLimit.trim() === "" ||
            tag.Data.Value.trim() === ""
        ) {
            return false;
        }
        return true;
    }

    const create = async () => {
        const type = "AOTag";
        const name = document.getElementsByName("name")[0].value;
        const description = document.getElementsByName("description")[0].value;
        const address = document.getElementsByName("address")[0].value;
        const units = document.getElementsByName("unit")[0].value;
        const lowLimit = document.getElementsByName("low-limit")[0].value;
        const highLimit = document.getElementsByName("high-limit")[0].value;
        const value = document.getElementsByName("value")[0].value;

        const tag = {
            Type: type,
            Data: {
                Name: name,
                Description: description,
                Address: address,
                Units: units,
                LowLimit: lowLimit,
                HighLimit: highLimit,
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
            <p id="title">New Analog Output Tag</p>

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
                name="description"
                className="input"
                placeholder="Type description here..."
            />

            <p className="label">Address</p>
            <input
                className="input"
                type="text"
                name="address"
                maxLength="10"
                placeholder="Type address here..."
            ></input>

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

            <p
                className="label">Value</p>
            <input
                className="input"
                type="number"
                name="value"
                maxLength="15"
                placeholder="Type value here..."
            ></input>


            <div id="buttons">
                <button className="btn" id="save" onClick={create}>SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default AOTag;