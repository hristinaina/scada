import '../AOTag/AOTag.css';


const DOTag = ({ onClose }) => {
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
            <textarea className="input" placeholder="Type description of the tag here..."
            />

            <p className="label">Address</p>
            <select className="input">
                <option value="fruit">Fruit</option>
                <option value="vegetable">Vegetable</option>
                <option value="meat">Meat</option>
            </select>


            <div id="buttons">
                <button className="btn" id="save">SAVE</button>
                <button className="btn" onClick={onClose}>CLOSE</button>
            </div>
        </div>
    );
};

export default DOTag;