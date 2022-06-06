var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',
    Search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = Sortable.baseUrl + "?id=" + searchKey;

    },
    Sort(sortBy) {
        var isDesc = true;

        // how to detect from the url if isDesc is true or false
        const urlParams = new URLSearchParams(window.location.search);

        var isDescOriginal = urlParams.get('isDesc');
        const sortByOriginal = urlParams.get('sortBy');

        // sorting the same column, means we need to change the isDesc value
        if (sortByOriginal == sortBy) {
            if (isDescOriginal == 'true') {
                isDesc = false;
            }

        }



        window.location.href = Sortable.baseUrl + "?sortBy=" + sortBy + "&isDesc=" + isDesc;

    }
};



var apiHandler = {
    GET(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (res) {
                debugger;
            }
        });
    },
    POST(url, object) {
        object = {
            Id: 5,
            Name: "asd",
            Email: "test@gmail.com",
        }
        $.ajax({
            url: url,
            type: 'GET',
            data: object,
            success: function (res) {
                debugger;
            }
        });
    },
    DELETE(url) {
        if (confirm("Are you sure you want to delete ?")) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (res) {
                    debugger;
                }
            });
        } else {
            alert("Ok, delete cancelled.");
        }

    }
};

