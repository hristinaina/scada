class TagService {

    static async getAOData() {
        // Simulirajte dohvat podataka sa API-ja
        const response = await fetch('http://localhost:5083/api/tag/ao');
        const data = await response.json();
        return data;
    }

    static async getDOData() {
        // Simulirajte dohvat podataka sa API-ja
        const response = await fetch('http://localhost:5083/api/tag/do');
        const data = await response.json();
        return data;
    }
}

export default TagService;

