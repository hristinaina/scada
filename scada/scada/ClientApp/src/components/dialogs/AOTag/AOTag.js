import './AOTag.css';


const AOTag = ({ onClose }) => {
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
            <textarea className="input" placeholder="Type description of the tag here..."
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
                name="high-limit"
                maxLength="15"
                placeholder="Type value here..."
            ></input>


            <div id="buttons">
                <button className="btn" id="save">SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default AOTag;