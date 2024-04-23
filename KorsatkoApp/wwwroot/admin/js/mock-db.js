class MockTable {
    validator;
    #data = {};
    #nextId = 1;
    #relations = {};
    
    constructor(validator, deleteChecker) {
        this.validator = validator;
        this.checkDelete = deleteChecker;
    }
    
    get(id) {
        return this.#data[id];
    }
    
    delete(id) {
        if (this.checkDelete) {
            this.checkDelete(this.get(id), id);
        }
        
        delete this.#data[id];
    }
    
    put(row) {
        if (validator) {
            row = validator(row);
        }
        
        if (row) {
            this.#data[this.#nextId++] = row;
        }
    }
    
    containsId(id) {
        return id in this.#data;
    }
}

class MockDB {
    #tables = {
        courses: new MockTable(),
        instructors: new MockTable(),
        sessions: new MockTable(),
        students: new MockTable(),
        registrations: new MockTable()
    };
    
    constructor() {
        this.#tables.instructors.checkDelete = row => {
            if (this.#tables.sessions.containsId(row.instructorId)) {
                
            }
        }
    }
    
    addCourse(courseName, courseCode, content, lengthWeeks, price) {
        MockDB.#ensureStringsFilled({ courseName, courseCode, content });
        MockDB.#ensureType({ lengthWeeks, price }, "number");
        
        lengthWeeks = Math.trunc(lengthWeeks);
        
        this.#tables.courses.put({ courseName, courseCode, content, lengthWeeks, price });
    }
    
    addInstructor(instructorName, numberOfCourses, phone, email) {
        MockDB.#ensureStringsFilled({ instructorName, phone, email });
        MockDB.#ensureType({ numberOfCourses, }, "number");
        
        numberOfCourses = Math.trunc(numberOfCourses);
        
        this.#tables.instructors.put({ instructorName, numberOfCourses, phone, email });
    }
    
    addSession(courseId, startDate, endDate, startTime, endTime, instructorId, sessionType) {
        MockDB.#ensureStringsFilled({ startDate, endDate, startTime, endTime, instructorId, sessionType });
        MockDB.#ensureType({ courseId });
        
        if (!this.#tables.courses.containsId(courseId)) {
            
        }
        
    }
    
    
    
    static #ensureStringsFilled(attributes) {
        for (let [k, v] in Object.entries(attributes)) {
            if (v == null) {
                throw new Error(k + " is required.");
            }
            if (typeof v !== "string") {
                throw new Error(k + " must be a string.");
            }
        }
    }
    
    static #ensureType(attributes, type) {
        for (let [k, v] in Object.entries(attributes)) {
            if (typeof v !== type) {
                throw new Error(k + " must be a number.");
            }
        }
    }
}
const mockDB = (function() {
    const exports = {};
    
    const tables = {
        courses: {},
        instructors: {},
        sessions: {},
        students: {},
        registrations: {}
    };
    
    function ensureStringsFilled(attributes) {
        for (let [k, v] in Object.entries(attributes)) {
            if (v == null) {
                throw new Error(k + " is required.");
            }
            if (typeof v !== "string") {
                throw new Error(k + " must be a string.");
            }
        }
    }
    
    function ensureType(attributes, type) {
        for (let [k, v] in Object.entries(attributes)) {
            if (typeof v !== type) {
                throw new Error(k + " must be a number.");
            }
        }
    }
    
    exports.addCourse = (courseName, courseCode, content, lengthWeeks, price) => {
        ensureStringsFilled({ courseName, courseCode, content });
        ensureType({ lengthWeeks, price });
        
        tables.courses
    }
    
    
    return exports;
})();