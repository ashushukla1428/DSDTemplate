var prefix = [], substring = [], matches = []

onmessage = function(message) {
	var data = message.data
	var term = data.term.trim().toLowerCase()
	var terms = term.split(' ').map(function (str) { return str.trim() })
	var idMatch, nameMatch, i, count = data.index.ids.length

	for (i = 0; i < count; i++) {
		let id = data.index.ids[i]
		let name = data.index.names[i].toLowerCase()
		let found = true
		idMatch = id.indexOf(term)
		if (idMatch === 0) {
			prefix.push(id)
		} else if(idMatch > 0) {
			substring.push(id)
		} else {
			nameMatch = name.indexOf(term)
			if (nameMatch === 0) {
				prefix.push(id)
			} else if (nameMatch > 0) {
				substring.push(id)
			} else {
				found = false
			}
		}

		if (!found && terms.length > 1) {
			found = true
			terms.forEach(function (subTerm) {
				if (name.indexOf(subTerm) === -1) {
					found = false
				}
			})

			if (found) {
				matches.push(id)
			}
		}
	}

	var result = prefix.concat(substring).concat(matches)

	postMessage(result)
}
