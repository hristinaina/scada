import './DITag.css';


const DITag = ({ onClose }) => {
    return (
        <div className="dialog">
            <p id="title">New Digital Input Tag</p>

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
            <textarea className="input" placeholder="Type description of the tag here..."
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
            <select className="input">
                <option value="real-time">Real-time</option>
                <option value="simulation">Simulation</option>
            </select>

            <div id="buttons">
                <button className="btn" id="save">SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default DITag;